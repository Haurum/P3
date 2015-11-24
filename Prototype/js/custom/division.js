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
  $scope.let = [];
  $scope.let[0] = "A";
  $scope.let[1] = "B";
  $scope.let[2] = "C";
  $scope.let[3] = "D";

  $scope.getDivisionData = function() {
    $http.get($rootScope.apiUrl + "/Division/Details?id=" +  $routeParams.divisionId)
    .success(function(data)
    {
      $scope.division = data;
      $scope.division.letters = [];
      
      for (var i = 0; i < $scope.division.Pools[0].Teams.length; i++)
      {
        $scope.division.letters.push({ nr: i, ch: $scope.allLetters[i]});
      }
      console.log($scope.division.tournamentId);
    }).error(function(err) 
    {
      $scope.error = err;
    })
  }
  $scope.getDivisionData();

  $scope.newPoolFunc = function() {
    $scope.newPool = !$scope.newPool;
  }  

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

  $scope.gotoPool = function(currPool, index) {
    $rootScope.currPoolIndex = index;
    $location.url($location.url() + "/pool/" + currPool.Id);
  }


  $scope.gotoTeam = function(currTeam, indexT, currPool, indexP) {
    $rootScope.currTeamIndex = indexT;
    $rootScope.currPoolIndex = indexP;
    $location.url($location.url() + "/pool/" + currPool.Id + "/team/" + currTeam.Id)
  }

$scope.gotoTournament = function() {
  $location.url("/tournament/" + $routeParams.tournamentId);
}

  $scope.changeDivNameFunc = function() {
    $scope.changeName = !$scope.changeName;
  }

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


  $scope.updateFinalstageLink = function(finalsLink, index)
  {
    $http.post($rootScope.apiUrl + "FinalsLink/Edit", { id: finalsLink.Id, finalsStage: "", poolPlacement: finalsLink.PoolPlacement })
    .success(function(data) {
      console.log(data);
    })
    .error(function(err) {
      $scope.uflErr = err;
    })
  }

  $scope.finalsLinkChanged = function(finalsLink, index)
  {
    console.log(finalsLink);
    console.log(index);
  }

}]);
