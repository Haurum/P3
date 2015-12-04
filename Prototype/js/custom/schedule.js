app.controller('ScheduleController', ['$scope', '$rootScope', '$location', '$http', '$routeParams', '$filter', function ($scope, $rootScope, $location, $http, $routeParams, $filter) {
  
  $scope.getData = function(){
    $http.get("http://localhost:50229/Field/GetAllTournamentFields?tournamentId=" + 1)
    .success(function(data)
    {
      $scope.fields = data;
      console.log("hej");
      //default state of the scheduler
      if (!$scope.scheduler)
        $scope.scheduler = {};
      $scope.scheduler.mode = $scope.scheduler.mode || "timeline";
      $scope.scheduler.date = $scope.scheduler.date || new Date();
      
      //watch data collection, reload on changes
     /* $scope.$watch($attrs.data, function(collection){
        scheduler.clearAll();
        scheduler.parse(collection, "json");
      }, true);*/
      
      //watch mode and date
      /*$scope.$watch(function(){
        return $scope.scheduler.mode + $scope.scheduler.date.toString();
      }, function(nv, ov) {
        var mode = scheduler.getState();
        if (nv.date != mode.date || nv.mode != mode.mode)
          scheduler.setCurrentView($scope.scheduler.date, $scope.scheduler.mode);
      }, true);*/
      

      //styling for dhtmlx scheduler

      //init scheduler
      scheduler.config.server_utc = true;
      scheduler.config.multi_day = true;
      
      var sections = [];
      
      for (var i=0; i < $scope.fields.Fields.length; i++)
      {
        sections.push({ key: $scope.fields.Fields[i].Id, label: $scope.fields.Fields[i].Name });
        console.log(sections[i]);
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
        name: "timeline",
        x_unit: "minute",
        x_date: "%H:%i",
        x_step: 60,
        x_size: 16,
        x_start: 8,
        x_length: 24,
        y_unit: sections,
        y_property: "section_id",
        render:"bar"
      });
      
      scheduler.init("dhx_cal_container", $scope.scheduler.date, $scope.scheduler.mode);
      var matches = [];
      
      for (var i=0; i < $scope.fields.Fields.length; i++)
      {
        for (var j=0; j < $scope.fields.Fields[i].matches.length; j++)
        {
          matches.push({ 
            start_date: $filter('jsonDate')($scope.fields.Fields[i].matches[j].StartTime), 
            end_date: $filter('jsonDate')($scope.fields.Fields[i].matches[j].EndTime), 
            text: $scope.fields.Fields[i].matches[j].Nr,  
            section_id: $scope.fields.Fields[i].Id });
        }
      }
      $scope.testMatch = matches[2];

      scheduler.parse(matches,"json");
    }).error(function (err) {
      $scope.error = err;
    })
  }   

  $scope.getData();
      
}]);