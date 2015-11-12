app.controller('TournamentController', ['$scope', '$rootScope', '$location', '$http', '$routeParams', function ($scope, $rootScope, $location, $http, $routeParams) {
  $rootScope.Tournament.password = "";

  $http.get("http://localhost:50229/Tournament/Details/" +  $routeParams.id)
  .success(function(data)
  {
    $scope.divisions = data.Divisions;
  }).error(function(err) 
  {
    $scope.error = err;
  })
  
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
    $location.url("/division");
  }
}]);

app.controller('CreateTournyController', ['$scope', '$rootScope', '$http', '$location', '$routeParams', function ($scope, $rootScope, $http, $location, $routeParams) {
  $scope.helper = true;
  $scope.withExcel = function() {
    if ($scope.helper)
    {
      $rootScope.divisions = [{ MatchDuration: "30", FieldSize: "11-mands", Name: "U17 Drenge A", Pool: [{ Name: "Pulje 1", Teams: ["Hjørring FC", "Aab", "Thisted FC", "Klinkby B"], IsOpen: false}, { Name: "Pulje 2", Teams: ["Randers", "FCK", "Hobro", "Lemvig"], IsOpen: false}]}, 
      { MatchDuration: "30", FieldSize: "11-mands", Name: "U17 Drenge B", Pool: [{ Name: "Pulje 1", Teams: ["FC Midtjylland", "Ab", "Thy FC", "Klinkerne"], IsOpen: false}, { Name: "Pulje 2", Teams: ["Anholdt", "FLY", "Holstebro", "Lemvig"], IsOpen: false}]}, 
      { MatchDuration: "30", FieldSize: "8-mands", Name: "U13 Pige A", Pool: [{ Name: "Pulje 1", Teams: ["Holstebro", "Klinkerne", "Thisted", "Aab"], IsOpen: false}, { Name: "Pulje 2", Teams: ["Chang", "Freja", "HIF", "FCLV"], IsOpen: false}]}, 
      { MatchDuration: "30", FieldSize: "8-mands", Name: "U13 Pige B", Pool: [{ Name: "Pulje 1", Teams: ["Aab B", "Hold A", "Fredericia", "Vejle B"], IsOpen: false}, { Name: "Pulje 2", Teams: ["Vejle A", "Kolding", "KFUM Aalborg", "KFUM Aarhus"], IsOpen: false}]}, 
      { MatchDuration: "30", FieldSize: "5-mands", Name: "U8 Drenge", Pool: [{ Name: "Pulje 1", Teams: ["Aab", "Hirsthals", "Thy FC", "Klinkby B"], IsOpen: false}, { Name: "Pulje 2", Teams: ["Aars", "KFUM Aalborg", "HIF", "FCL"], IsOpen: false}]}];
      $rootScope.EmFields = ["Bane 1", "Bane 2", "Bane 5", "Bane 6"];
      $rootScope.OmFields = ["Bane 3A", "Bane 3B", "Bane 4A", "Bane 4B"];
      $rootScope.FmFields = ["Bane 7A", "Bane 7B", "Bane 7C", "Bane 7D"];
    }
    $scope.helper = !$scope.helper;
  }
  if ($scope.withExcel)
  {
    
  }

  $scope.getTournamentData = function()
  {
    var tournamentData = {
      tournamentName: $scope.tournamentName,
      tournamentPassword: $scope.tournamentName,
      tournamentStartDate: $scope.sd,
      tournamentEndDate: $scope.ed
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

  /* DATE PICKER START */
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
  /* DATE PICKER END */ 


}]);