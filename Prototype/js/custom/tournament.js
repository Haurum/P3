app.controller('TournamentController', ['$scope', '$rootScope', '$location', '$http', '$routeParams', '$uibModal', function ($scope, $rootScope, $location, $http, $routeParams, $uibModal) {
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
  $scope.submitNewDiv = function(newDivName, newMatchDuration, chooseField) {
    $http.post("http://localhost:50229/Division/Create", { Name: newDivName, MatchDuration: newMatchDuration, FieldSize: chooseField, tournamentId: $routeParams.tournamentId })
    .success(function(data){
      $scope.newDivName = "";
      $scope.newMatchDuration = "";
      $scope.chooseField = "";
      $scope.getDivisions();
      $scope.createNew();
    }).error(function(data){
      $scope.newDivError = data;
    })   
  }


  /* Modal start */
  $scope.animationsEnabled = true;

  $scope.open = function (size) {

    var uibModalInstance = $uibModal.open({
      animation: $scope.animationsEnabled,
      templateUrl: 'myModalContent.html',
      controller: 'ModalInstanceCtrl',
      size: size,
      /*resolve: {
        items: function () {
          return $scope.items;
        }
      }*/
    });

    uibModalInstance.result.then(function () {
      $scope.getDivisions();
        $scope.createNew();
    }, function () {
      $log.info('Modal dismissed at: ' + new Date());
    });

  };



  $scope.toggleAnimation = function () {
    $scope.animationsEnabled = !$scope.animationsEnabled;
  };
  /* Modal end */


  $scope.gotoDivison = function (currDiv, index) {
    $rootScope.currDivisionIndex = index;
    console.log("tournament/" + $routeParams.tournamentId+ "/division/" + currDiv.Id);
    $location.url("tournament/" + $routeParams.tournamentId+ "/division/" + currDiv.Id);
  }

}]);

app.controller('ModalInstanceCtrl', ['$scope', '$uibModalInstance', '$http', '$routeParams', function ($scope, $uibModalInstance, $http, $routeParams) {

    $scope.newDivName = "";
    $scope.newMatchDuration = "";
    $scope.chooseField = "";
  $scope.submitNewDiv = function(newDivName, newMatchDuration, chooseField) {
    $http.post("http://localhost:50229/Division/Create", { Name: newDivName, MatchDuration: newMatchDuration, FieldSize: chooseField, tournamentId: $routeParams.tournamentId })
      .success(function(data){
        
        $uibModalInstance.close();
        $scope.newDivName = "";
        $scope.newMatchDuration = "";
        $scope.chooseField = "";
        
      }).error(function(data){
        $scope.newDivError = data;
      })
    } 

  $scope.ok = function () {
    $uibModalInstance.close();
  };

  $scope.cancel = function () {
    $uibModalInstance.dismiss('cancel');
  };
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
  $scope.startTimes = [];
  $scope.endTimes = [];

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
    for (var i = 0; i <= $scope.dateRange; i++) {
      var date = new Date($scope.startDate.getTime());
      date.setDate(date.getDate() + i);
      $scope.dateArray.push(date);
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
    if ($scope.tournamentName && $scope.tournamentPassword)
    {
      $scope.startDateTimes = [];
      $scope.endDateTimes = [];
      $scope.error = false;
      for (var index = 0; index < $scope.startTimes.length; index++) {
        var startDateTime = new Date($scope.dateArray[index].getTime() + $scope.startTimes[index].getHours() * 60 * 60 * 1000 + $scope.startTimes[index].getMinutes() * 60 * 1000);
        $scope.startDateTimes[index] = startDateTime.toISOString();
        
        var endDateTime = new Date($scope.dateArray[index].getTime() + $scope.endTimes[index].getHours() * 60 * 60 * 1000 + $scope.endTimes[index].getMinutes() * 60 * 1000);
        $scope.endDateTimes[index] = endDateTime.toISOString();
        
        if($scope.startDateTimes[index] === {} && $scope.endDateTimes[index] === null){
          $scope.error = "en start eller slut tid er ikke sat";
          break;
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
          }
        }).error(function(err) 
        {
          $scope.error = "kunne ikke uploade til serveren";
        });
      }    
    }else{
      $scope.error = "navn eller kode ikke sat";
    }   
    
  }

}]);
