var app = angular.module('tournyplanner', ['ngRoute', 'ui.bootstrap', 'angularFileUpload', 'angular-loading-bar']);

app.config(['$routeProvider', function($routeProvider) {
  $routeProvider.
    when('/', {
      templateUrl: 'templates/home.html',
      controller: 'HomeController'
    }).
    when('/addTournament', {
      templateUrl: 'templates/addTournament.html',
      controller: 'CreateTournyController'
    }).
    when('/tournament/:tournamentId/edit', {
      templateUrl: 'templates/editTournament.html',
      controller: 'EditTournamentController'
    }).
    when('/tournament/:tournamentId', {
      templateUrl: 'templates/tournament.html',
      controller: 'TournamentController'
    }).
    when('/tournament/:tournamentId/division/:divisionId', {
      templateUrl: 'templates/division.html',
      controller: 'DivisionController'
    }).
    when('/tournament/:tournamentId/division/:divisionId/pool/:poolId', {
      templateUrl: 'templates/pool.html',
      controller: 'PoolController'
    }).
    when('/tournament/:tournamentId/field/:fieldId', {
      templateUrl: 'templates/field.html',
      controller: 'TournamentController'
    }).
    when('/tournament/:tournamentId/division/:divisionId/pool/:poolId/team/:teamId', {
      templateUrl: 'templates/team.html',
      controller: 'TeamDetailController'
    }).
     when('/schedule', {
      templateUrl: 'templates/schedule.html',
      controller: 'ScheduleController'
    }).   
    otherwise({
      redirectTo: '/'
    });
}]);

app.config(['cfpLoadingBarProvider', function(cfpLoadingBarProvider) {
  cfpLoadingBarProvider.includeSpinner = true;
  cfpLoadingBarProvider.parentSelector = '#navbar';
}]);

app.run(function($rootScope) {
  $rootScope.divisions = [];
  $rootScope.EmFields = [];
  $rootScope.OmFields = [];
  $rootScope.FmFields = [];
  $rootScope.Tournament = {};
  $rootScope.apiUrl = "http://localhost:50229";
});

app.controller('HomeController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
  $scope.password = "";

  $scope.getId = function(password)
  {
    $http.post("http://localhost:50229/Tournament/IdFromPass", { password: password })
    .success(function(passwordData)
    {
      if (passwordData.Id != 0)
      {
        $scope.error = false;
        $location.path("tournament/" + passwordData.Id);
      }
      else{
        $scope.error = true;
      }
    }).error(function(err) 
    {
      $scope.error = err;
    });
  }
}]);

app.directive('dhxScheduler', function() {
  return {
    restrict: 'A',
    scope: false,
    transclude: true,
    template:'<div class="dhx_cal_navline" ng-transclude></div><div class="dhx_cal_header"></div><div class="dhx_cal_data"></div>',

    link:function ($scope, $element, $attrs, $controller){
      
      //default state of the scheduler
      if (!$scope.scheduler)
        $scope.scheduler = {};
      $scope.scheduler.mode = $scope.scheduler.mode || "timeline";
      $scope.scheduler.date = $scope.scheduler.date || new Date();
      
      //watch data collection, reload on changes
      $scope.$watch($attrs.data, function(collection){
        scheduler.clearAll();
        scheduler.parse(collection, "json");
      }, true);
      
      //watch mode and date
      $scope.$watch(function(){
        return $scope.scheduler.mode + $scope.scheduler.date.toString();
      }, function(nv, ov) {
        var mode = scheduler.getState();
        if (nv.date != mode.date || nv.mode != mode.mode)
          scheduler.setCurrentView($scope.scheduler.date, $scope.scheduler.mode);
      }, true);
      

      //styling for dhtmlx scheduler
      $element.addClass("dhx_cal_container");

      //init scheduler
      scheduler.config.server_utc = true;
      scheduler.config.multi_day = true;
      
      var sections = [];
      
      for (var i=0; i < $scope.tournament.Fields.length; i++)
      {
        sections.push({ key: $scope.tournament.Fields[i].Id, label: $scope.tournament.Fields[i].Name })
        
      }
      
      scheduler.locale.labels.timeline_tab = "Timeline"
		  scheduler.locale.labels.section_custom="Section";
		  scheduler.config.fix_tab_position = false;
		  scheduler.config.details_on_create=true;
		  scheduler.config.details_on_dblclick=true;
		  scheduler.config.xml_date="%Y-%m-%d %H:%i";
		  
		  scheduler.config.first_hour = 6
		  scheduler.config.last_hour = 23
		  
		  scheduler.config.lightbox.sections=[	
        {name:"description", height:130, map_to:"text", type:"textarea" , focus:true},
        {name:"custom", height:23, type:"select", options:sections, map_to:"section_id" },
        {name:"time", height:72, type:"time", map_to:"auto"}
      ]
      
      scheduler.createTimelineView({
			  name:	"timeline",
			  x_unit:	"minute",
			  x_date:	"%H:%i",
			  x_step:	60,
			  x_size: 12,
			  x_start: 8,
			  x_length:	24,
			  y_unit:	sections,
			  //y_property:	"section_id",
			  render:"bar"
		  });
		  
		  scheduler.init($element[0], $scope.scheduler.date, $scope.scheduler.mode);
		  
		  scheduler.parse([
        { start_date: "2012-08-30 09:00", end_date: "2012-08-30 12:00", text:"Task A-12458", section_id:1},

        
        { start_date: "2012-08-30 12:00", end_date: "2012-08-30 20:00", text:"Task B-48865", section_id:2}
      ],"json");
    }
  }
});
