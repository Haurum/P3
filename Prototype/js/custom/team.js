app.controller('TeamDetailController', ['$scope', '$rootScope', '$location', '$http', '$routeParams', function ($scope, $rootScope, $location, $http, $routeParams) {
  $scope.changeName = false;
  $scope.orderByField = 'Number';
  $scope.reverseSort = false;

  $scope.getTeamData = function() {
    $http.get($rootScope.apiUrl + "/Team/Details?id=" +  $routeParams.teamId)
    .success(function(data)
    {
      $scope.team = data;
      console.log(data);
      console.log($scope.team.TimeIntervals);
      $scope.dateArray = [];
      $scope.startTimes = [];
      $scope.endTimes = [];
      
      $scope.startDate = new Date(parseInt(data.TimeIntervals[0].StartTime.substr(6)));
      $scope.startDate.setHours(0);
      $scope.startDate.setSeconds(0);
      $scope.startDate.setMinutes(0);
      $scope.startDate.setMilliseconds(0);
      $scope.endDate = new Date(parseInt(data.TimeIntervals[data.TimeIntervals.length-1].EndTime.substr(6)));
      $scope.endDate.setHours(0);
      $scope.endDate.setSeconds(0);
      $scope.endDate.setMinutes(0);
      $scope.endDate.setMilliseconds(0);
      $scope.dateRange = ($scope.endDate - $scope.startDate) / (1000 * 60 * 60 * 24);
      console.log($scope.startDate);
      console.log($scope.endDate);

      for (var index = 0; index < data.TimeIntervals.length; index++) {
        var date = new Date($scope.startDate.getTime());
        date.setDate(date.getDate() + index);
        $scope.dateArray.push(date);
        $scope.startTimes.push(new Date(parseInt(data.TimeIntervals[index].StartTime.substr(6))));
        $scope.endTimes.push(new Date(parseInt(data.TimeIntervals[index].EndTime.substr(6))));
      }
    }).error(function(err) 
    {
      $scope.error = err;
    })
  }
  $scope.getTeamData();

  $scope.changeTeamNameFunc = function() {
    $scope.changeName = !$scope.changeName;
  }

  $scope.changeNewPoolNameFunc = function(newName) {
    $http.post($rootScope.apiUrl + "/Team/Edit", { name: newName, id: $routeParams.teamId, poolId: $routeParams.poolId, startTimes: $scope.team.StartTime, endTimes: $scope.team.EndTime})
    .success(function(data){
      console.log(data);
      $scope.pool.Name = data.newName;
      $scope.changeTeamNameFunc();
      $scope.getTeamData();
    }).error(function(err){
      $scope.editErr = err;
    })
  }

  $scope.remove = function() {
    $http.post($rootScope.apiUrl + "/Team/Delete", { id: $routeParams.teamId })
    .success(function(data) {
      console.log(data);
      $location.path("/tournament/" + $routeParams.tournamentId + "/division/" + $routeParams.divisionId + "/pool/" + $routeParams.poolId);
    }).error(function(data) {
      $scope.deleteErr = data;
    })   
  }

  $scope.gotoDivision = function() {
    $location.url("/tournament/" + $routeParams.tournamentId + "/division/" + $routeParams.divisionId);
  }

  $scope.gotoTournament = function() {
    $location.url("/tournament/" + $routeParams.tournamentId);
  }

  $scope.gotoPool = function() {
    $location.url("/tournament/" + $routeParams.tournamentId + "/division/" + $routeParams.divisionId + "/pool/" + $routeParams.poolId);
  }

  /* Change time intervals start */
    
  
  $scope.toggleMin = function () {
    $scope.minDate = $scope.minDate ? null : new Date();
  };
  $scope.toggleMin();
  $scope.maxDate = new Date(2020, 5, 22);
  $scope.openEndDate = function ($event) {
    $scope.statusEndDate.opened = true;
  };

  $scope.isChanged = function () {
    $scope.dateRange = ($scope.endDate - $scope.startDate) / (1000 * 60 * 60 * 24);
    $scope.dateArray = [];
    $scope.startTimes = [];
    $scope.endTimes = [];
    for (var i = 0; i <= $scope.dateRange; i++) {
      var date = new Date($scope.startDate.getTime());
      date.setDate(date.getDate() + i);
      $scope.dateArray.push(date);
      $scope.startTimes.push(date);
      $scope.endTimes.push(date);
    }
  }

  $scope.openStartDate = function ($event) {
    $scope.statusStartDate.opened = true;
  };
  $scope.dateOptions = {
    formatYear: 'yy',
    startingDay: 1
  };
  $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
  $scope.format = $scope.formats[0];
  $scope.statusStartDate = {
    opened: false
  };
  $scope.statusEndDate = {
    opened: false
  };
  
  $scope.uploadTeam = function () {
    if (!$scope.team.Name){
      $scope.error = "Navn er ikke sat";
    }else{
      $scope.startDateTimes = [];
      $scope.endDateTimes = [];
      $scope.error = false;
      console.log($scope.dateRange);
      console.log($scope.startTimes);
      for (var index = 0; index < $scope.startTimes.length; index++) {
        $scope.startDateTimes[index] = $scope.startTimes[index];
        $scope.endDateTimes[index] = $scope.endTimes[index];        
        
      }
      
      console.log($scope.startDateTimes);

      if($scope.startDateTimes.length-1 !== $scope.dateRange && $scope.endDateTimes.length-1 !== $scope.dateRange){
        console.log($scope.startDateTimes.length);
        console.log($scope.dateRange);
        $scope.error = "Fejl i start eller slut tidspunkt for en af dagene";
          
      }else{
        for(var i = 0; i <= $scope.dateRange; i++){
          if($scope.startDateTimes[i] >= $scope.endDateTimes[i]){
            $scope.error = "alle slut tidspunkter skal være senere end start tidspunkter";
          }
        }
        if(!$scope.error){
          var teamData = {
            id: $scope.team.Id,
            name: $scope.team.Name,
            poolId: $routeParams.poolId,
            startTimes: $scope.startDateTimes,
            endTimes: $scope.endDateTimes
          }
      
          $http.post("http://localhost:50229/Team/Edit/", teamData)
          .success(function(Data)
          {
            $scope.isSuccess = true;
            $scope.getTeamData();
          }).error(function(err)
          {
            $scope.isSuccess = false;
            $scope.error = err;
            $scope.getTeamData();
          });
          }
        }  
      }   
  }


  /* Change time intervals end */ 

}]);