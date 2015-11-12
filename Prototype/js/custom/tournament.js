app.controller('TournamentController', ['$scope', '$rootScope', '$location', '$http', '$routeParams', function ($scope, $rootScope, $location, $http, $routeParams) {
  $rootScope.Tournament.password = "";

  $scope.getDivisions = function(){
    $http.get("http://localhost:50229/Tournament/Details" +  $routeParams.tournamentId)
      .success(function(data)
      {
        $scope.divisions = data.Divisions;
      }).error(function(err) 
      {
        $scope.error = err;
    })
  }

  $scope.getDivisions();
  
  $scope.newDivName = "";
  
  $scope.new = false;
  
  $scope.createNew = function() {
    $scope.new = !$scope.new;
  }
  $scope.submit = function(newName) {
    $rootScope.divisions.push({ MatchDuration: "30", FieldSize: "11-mands", Name: newName, Pool: []});
    $scope.newDivName = "";
    $scope.createNew();
  }
  
  $scope.gotoDivison = function(currDiv, index) {
    $rootScope.currDivisionIndex = index;
    $location.url("tournament/" + $routeParams.tournamentId+ "/division/" + currDiv.Id);
  }
}]);

app.controller('CreateTournyController', ['$scope', '$rootScope', '$http', '$location', '$routeParams', function ($scope, $rootScope, $http, $location, $routeParams) {

 
  
  
  /* DATE PICKER START */
  $scope.dateRange = 0;
  $scope.today = function() {
    $scope.startDate = new Date();
    $scope.startDate.setHours(0);
    $scope.startDate.setSeconds(0);
    $scope.startDate.setMinutes(0);
    $scope.startDate.setMilliseconds(0);
    $scope.endDate = new Date(); 
    $scope.endDate.setHours(0);
    $scope.endDate.setSeconds(0);
    $scope.endDate.setMinutes(0);
    $scope.endDate.setMilliseconds(0);
  };
  $scope.today();
  
  $scope.dateArray = [$scope.startDate];
  
  $scope.toggleMin = function() {
    $scope.minDate = $scope.minDate ? null : new Date();
  };
  $scope.toggleMin();
  $scope.maxDate = new Date(2020, 5, 22);
  $scope.openEndDate = function($event) {
    $scope.statusEndDate.opened = true;
  };
  
  $scope.isChanged = function()
  {
    $scope.dateRange = ($scope.endDate-$scope.startDate)/(1000*60*60*24);
    $scope.dateArray = [];
    for (var i = 0; i <= $scope.dateRange; i++)
    {
      var date = new Date($scope.startDate.getTime());
      date.setDate(date.getDate() + i+1);
      $scope.dateArray.push(date);
    }
  }
  
  $scope.openStartDate = function($event) {
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
  /* DATE PICKER END */ 
  
  
  

  $scope.uploadTournament = function()
  {
    var tournamentData = {
      tournamentName: $scope.tournamentName,
      tournamentPassword: $scope.tournamentPassword,
      tournamentStartDate: $scope.startDate,
      tournamentEndDate: $scope.endDate
    }

    $http.post("http://localhost:50229/addTournament/", tournamentData).success(function(tournamentData)
    {
      if (tournamentData.tournamentName != 0 && tournamentData.password != 0 && tournamentData.tournamentStartDate != 0 && tournamentData.tournamentEndDate != 0)
      {
        $scope.error = false;
        $location.path("tournament/1");
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
