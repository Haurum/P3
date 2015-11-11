var app = angular.module('tournyplanner', ['ngRoute', 'ui.bootstrap'])

.config(['$routeProvider', function($routeProvider) {
  $routeProvider.
    when('/', {
      templateUrl: 'templates/home.html',
      controller: 'HomeController'
    }).
    when('/addTournament', {
      templateUrl: 'templates/addTournament.html',
      controller: 'CreateTournyController'
    }).
    when('/tournament/:id', {
      templateUrl: 'templates/tournament.html',
      controller: 'TournamentController'
    }).
    when('/division/:id', {
      templateUrl: 'templates/division.html',
      controller: 'DivisonController'
    }).
    when('/pool/:id', {
      templateUrl: 'templates/pool.html',
      controller: 'PoolController'
    }).
    when('/field', {
      templateUrl: 'templates/field.html',
      controller: 'CreateFieldsController'
    }).
    when('/team/:id', {
      templateUrl: 'templates/team.html',
      controller: 'TeamDetailController'
    }).   
    otherwise({
      redirectTo: '/'
    });
}])

.run(function($rootScope) {
  $rootScope.divisions = [];
  $rootScope.EmFields = [];
  $rootScope.OmFields = [];
  $rootScope.FmFields = [];
  $rootScope.Tournament = {};
})

.controller('HomeController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
  $scope.password = "";

  $scope.getId = function(password)
  {
    $http.post("http://localhost:50229/Tournament/IdFromPass", { password: password })
    .success(function(passwordData)
    {
      if (passwordData.Id != 0)
      {
        $scope.error = false;
        console.log($location.url);
        $location.path("tournament/" + passwordData.Id);
        console.log($location.url);
      }
      else{
        $scope.error = true;
      }
    }).error(function(err) 
    {
      $scope.error = err;
    });
  }
}])

/* DATE PICKER START! */

.controller('DatepickerDemoCtrl', function ($scope) {
  $scope.today = function() {
    $scope.st = new Date();
    $scope.et = new Date(); 
  };
  $scope.today();
  $scope.clear = function () {
    $scope.dt = null;
  };
  $scope.toggleMin = function() {
    $scope.minDate = $scope.minDate ? null : new Date();
  };
  $scope.toggleMin();
  $scope.maxDate = new Date(2020, 5, 22);
  $scope.openEt = function($event) {
    $event.preventDefault();
    $event.stopPropagation();
    $scope.statusEt.opened = true;
  };
  $scope.openSt = function($event) {
    $event.preventDefault();
    $event.stopPropagation();
    $scope.statusSt.opened = true;
  };
  $scope.setDate = function(year, month, day) {
    $scope.dt = new Date(year, month, day);
  };
  $scope.dateOptions = {
    formatYear: 'yy',
    startingDay: 1
  };
  $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
  $scope.format = $scope.formats[0];
  $scope.statusSt = {
    opened: false
  };
  $scope.statusEt = {
    opened: false
  };
  var tomorrow = new Date();
  tomorrow.setDate(tomorrow.getDate() + 1);
  var afterTomorrow = new Date();
  afterTomorrow.setDate(tomorrow.getDate() + 2);
  $scope.events =
    [
      {
        date: tomorrow,
        status: 'full'
      },
      {
        date: afterTomorrow,
        status: 'partially'
      }
    ];
  $scope.getDayClass = function(date, mode) {
    if (mode === 'day') {
      var dayToCheck = new Date(date).setHours(0,0,0,0);
      for (var i=0;i<$scope.events.length;i++){
        var currentDay = new Date($scope.events[i].date).setHours(0,0,0,0);
        if (dayToCheck === currentDay) {
          return $scope.events[i].status;
        }
      }
    }
    return '';
  };
})