app.controller('TournamentController', ['$scope', '$rootScope', '$location', '$http', '$routeParams', function ($scope, $rootScope, $location, $http, $routeParams) {
  $rootScope.Tournament.password = "";


  $scope.getDivisions = function(){
    $http.get("http://localhost:50229/Tournament/Details?id=" +  $routeParams.tournamentId)
      .success(function(data)
      {
        $scope.divisions = data.Divisions;
      }).error(function (err) {
        $scope.error = err;
      })
  }

  $scope.getDivisions();

  $scope.newDivName = "";
  $scope.chooseField = "";
  $scope.newMatchDuration = "";
  
  $scope.new = false;

  $scope.createNew = function () {
    $scope.new = !$scope.new;
  }
  $scope.submit = function(newDivName, newMatchDuration, chooseField) {
    $http.post("http://localhost:50229/Division/Create", { newDivName: newDivName, newMatchDuration: newMatchDuration, chooseField: chooseField } )
    $scope.newDivName = "";
    $scope.newMatchDuration = "";
    $scope.chooseField = "";
    $scope.createNew();
  }

  $scope.gotoDivison = function (currDiv, index) {
    $rootScope.currDivisionIndex = index;
    console.log("tournament/" + $routeParams.tournamentId+ "/division/" + currDiv.Id);
    $location.url("tournament/" + $routeParams.tournamentId+ "/division/" + currDiv.Id);
  }

}]);

app.controller('CreateTournyController', ['$scope', '$rootScope', '$http', '$location', '$routeParams', function ($scope, $rootScope, $http, $location, $routeParams) {

 
  
  
  /* DATE PICKER START */
  $scope.dateRange = 0;
  $scope.today = function () {
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
  $scope.startTimes = [$scope.startDate];
  $scope.endTimes = [$scope.startDate];

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
  /* DATE PICKER END */





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
            name: $scope.tournamentName,
            password: $scope.tournamentPassword,
            startTimes: $scope.startDateTimes,
            endTimes: $scope.endDateTimes
          }
      
          $http.post("http://localhost:50229/Tournament/Create/", tournamentData).success(function(Data)
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
}]);
