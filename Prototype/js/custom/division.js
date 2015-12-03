app.controller('DivisionController', ['$scope', '$rootScope', '$location', '$http', '$routeParams', function ($scope, $rootScope, $location, $http, $routeParams) {
  $scope.changeField = false;
  $scope.changeDuration = false;
  $scope.changeFavField = false;
  $scope.changeName = false;
  $scope.newPool = false;
  $scope.newPoolName = "";
  $scope.allLetters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
  $scope.division = "";
  $scope.division.letters = [];
  $scope.orderByField = 'Number';
  $scope.reverseSort = false;

  $scope.getDivisionData();

  //Getting all the data for the divisions, by using a get request to the backend, which will send back the data from the database.
  $scope.getDivisionData = function() {
    $http.get($rootScope.apiUrl + "/Division/Details?id=" +  $routeParams.divisionId)
    .success(function(data)
    {
      $scope.division = data;
      $scope.division.letters = [];
      
      for (var i = 0; i < $scope.division.FinalsLinks.length; i++)
      {
        $scope.division.letters.push($scope.allLetters[i]);
      }
      console.log($scope.division.tournamentId);
    }).error(function(err) 
    {
      $scope.error = err;
    })
  }

  $scope.newPoolFunc = function() {
    $scope.newPool = !$scope.newPool;
  }

  //Adding a new pool to the division. Making a post request to the backend to call the Create function in PoolController.
  $scope.addPool = function(name) {
    $http.post($rootScope.apiUrl + "/Pool/Create", { name: name, divisionId: $routeParams.divisionId })
    .success(function(data) {
      $scope.newPoolFunc();
      $scope.getDivisionData();
    }).error(function(err){
     $scope.deleteErr = err;
    })   
    $scope.getDivisionData();
  }
  
  //Deleting the specific division. Making a post request to the backend to call the Delete function DivisionController.
  $scope.remove = function() {
    $http.post($rootScope.apiUrl + "/Division/Delete", { id: $routeParams.divisionId })
    .success(function(data) {
      $location.path("/tournament/" + $routeParams.tournamentId);
    }).error(function(data) {
      $scope.deleteErr = data;
    })   
  }
  
  $scope.changeFieldFunc = function() {
    $scope.changeField = !$scope.changeField;
  }

  //This function is used the change the fieldsize of the divsion. Done by making a post request ot the backend which calls the Edit function
  //in DivisionController, and sends the required parameters with it.
  $scope.changeNewFieldFunc = function(editField) {
    $http.post($rootScope.apiUrl + "/Division/Edit", { name: $scope.division.Name, fieldSizeInt: editField, tournamentId: $routeParams.tournamentId, id: $routeParams.divisionId, matchDuration: $scope.division.MatchDuration  })
    .success(function(data){
      console.log(data);
      $scope.division.FieldSize = data.editField;
      $scope.changeFieldFunc();
      $scope.getDivisionData();
    }).error(function(err){
      $scope.editErr = err;
    })
  }

  $scope.changeDurationFunc = function() {
    $scope.changeDuration = !$scope.changeDuration;
  }

  //This function is used the change the matchduration of the divsion. Done by making a post request ot the backend which calls the Edit function
  //in DivisionController, and sends the required parameters with it.
  $scope.changeNewDurationFunc = function(editMatchDuration) {
    $http.post($rootScope.apiUrl + "/Division/Edit", { name: $scope.division.Name, fieldSizeInt: $scope.division.FieldSize, tournamentId: $routeParams.tournamentId, id: $routeParams.divisionId, matchDuration: editMatchDuration  })
    .success(function(data){
      console.log(data);
      $scope.division.MatchDuration = data.editMatchDuration;
      $scope.changeDurationFunc();
      $scope.getDivisionData();
    }).error(function(err){
      $scope.editErr = err;
    })
  }

  //Function used to to go a specific pool
  $scope.gotoPool = function(currPool, index) {
    $rootScope.currPoolIndex = index;
    $location.url($location.url() + "/pool/" + currPool.Id);
  }

  //Function used to to go a specific team
  $scope.gotoTeam = function(currTeam, currPool) {
    $location.url($location.url() + "/pool/" + currPool.Id + "/team/" + currTeam.Id);
  }

  //Function used to to go the tournament overview page
  $scope.gotoTournament = function() {
    $location.url("/tournament/" + $routeParams.tournamentId);
  }

  $scope.changeDivNameFunc = function() {
    $scope.changeName = !$scope.changeName;
  }

  //This function is used the change the name of the divsion. Done by making a post request ot the backend which calls the Edit function
  //in DivisionController, and sends the required parameters with it.
  $scope.changeNewDivNameFunc = function(newName) {
    $http.post($rootScope.apiUrl + "/Division/Edit", { name: newName, fieldSizeInt: $scope.division.FieldSize, tournamentId: $routeParams.tournamentId, id: $routeParams.divisionId, matchDuration: $scope.division.MatchDuration  })
    .success(function(data){
      $scope.division.Name = data.newName;
      $scope.changeDivNameFunc();
      $scope.getDivisionData();
    }).error(function(err){
      $scope.editErr = err;
    })
  }

  $scope.finalsLinkChanged = function(finalsLink)
  {
    console.log(finalsLink.FinalStage);
    console.log($scope.allLetters.indexOf(finalsLink.FinalStage));
    console.log({ id: finalsLink.Id, finalStage: $scope.allLetters.indexOf(finalsLink.FinalStage) + 1, poolPlacement: finalsLink.PoolPlacement  });
    /*$http.post($rootScope.apiUrl + "FinalsLink/Edit", { id: finalsLink.Id, finalStage: finalsLink.FinalStage, PoolPlacement: $scope.division.letters.indexOf(finalsLink.PoolPlacement) + 1 })
    .success(function(data) {
      console.log(data);
    })
    .error(function(err) {
      $scope.uflErr = err;
    })*/
  }

  $scope.isScheduled = false;
  $scope.schedule = function () {
    $scope.isScheduled = !$scope.isScheduled;
  }

}]);
