app.controller('TeamDetailController', ['$scope', '$rootScope', '$location', '$http', '$routeParams', function ($scope, $rootScope, $location, $http, $routeParams) {
  $scope.changeName = false;

  $scope.getTeamData = function() {
    $http.get($rootScope.apiUrl + "/Team/Details?id=" +  $routeParams.teamId)
    .success(function(data)
    {
      $scope.team = data;
      console.log(data);
      console.log($scope.team.StartTime);
    }).error(function(err) 
    {
      $scope.error = err;
    })
  }
  $scope.getTeamData();

  $scope.changePoolNameFunc = function() {
    $scope.changeName = !$scope.changeName;
  }

  $scope.changeNewPoolNameFunc = function(newName) {
    $http.post($rootScope.apiUrl + "/Team/Edit", { name: newName, id: $routeParams.teamId, poolId: $routeParams.poolId, startTimes: $scope.team.StartTime, endTimes: $scope.team.EndTime})
    .success(function(data){
      console.log(data);
      $scope.pool.Name = data.newName;
      $scope.changePoolNameFunc();
      $scope.getPoolData();
    }).error(function(err){
      $scope.editErr = err;
    })
  }

  /* Change time intervals start */
  $http.get("http://localhost:50229/Tournament/Details?id=" + $routeParams.tournamentId).success(function(data){
    
    if(data.status === "success"){
    
      $scope.tournamentId = data.Id;
      $scope.tournamentName = data.Name;
      $scope.tournamentPassword = data.Password;
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
      
      for (var index = 0; index < data.TimeIntervals.length; index++) {
        var date = new Date($scope.startDate.getTime());
        date.setDate(date.getDate() + index);
        $scope.dateArray.push(date);
        $scope.startTimes.push(new Date(parseInt(data.TimeIntervals[index].StartTime.substr(6))));
        $scope.endTimes.push(new Date(parseInt(data.TimeIntervals[index].EndTime.substr(6))));
      }
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
      
      $scope.uploadTournament = function () {
        if (!$scope.tournamentName || !$scope.tournamentPassword){
          $scope.error = "Navn eller kode ikke sat";
        }else{
          $scope.startDateTimes = [];
          $scope.endDateTimes = [];
          $scope.error = false;
          for (var index = 0; index <= $scope.dateRange; index++) {
            $scope.startDateTimes[index] = $scope.startTimes[index].toISOString();
            $scope.endDateTimes[index] = $scope.endTimes[index].toISOString();        
            
          }
          
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
              var tournamentData = {
                id: $scope.tournamentId,
                name: $scope.tournamentName,
                password: $scope.tournamentPassword,
                startTimes: $scope.startDateTimes,
                endTimes: $scope.endDateTimes
              }
          
              $http.post("http://localhost:50229/Tournament/Edit/", tournamentData).success(function(Data)
              {
                if(Data.status === "error"){
                  $scope.error = Data.message;
                }else{
                  $location.path("tournament/" + Data.Id);
                }
              }).error(function(err) 
              {
                $scope.error = "Kunne ikke uploade til serveren";
              });
            }
          }  
        }   
      }
    }else{
      $scope.error = "Kunne ikke finde turneringen";
    }
  }).error(function(err){
    $scope.error = "Kunne ikke finde turneringen";
  });


  /* Change time intervals end */ 

}]);