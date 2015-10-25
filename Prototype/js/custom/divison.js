angular.module('tournyplanner').controller('DivisonDetailController', ['$scope', '$rootScope', function ($scope, $rootScope) {
  $scope.division = { MatchDuration: "30", FieldSize: "11-mands", Name: ""};
  $scope.division.Name = $rootScope.currDivision;
  $scope.changeField = false;
  $scope.changeDuration = false;
  $scope.newPool = false;
  $scope.newPoolName = "";
  
  $scope.newPoolFunc = function() {
    $scope.newPool = !$scope.newPool;
  }
  
  $scope.pools = [];
  
  $scope.addPool = function(name) {
    $scope.newPoolName = "";
    if (name != "")
    {
      $scope.pools.push({ Name: name, Teams: [], IsOpen: false });
      $scope.newPoolFunc();
    }

  }
  
  $scope.addTeamToPool = function(newTeamName, index) {
    $scope.pools[index].Teams.push(newTeamName);
  }
  
  $scope.changeFieldFunc = function() {
    $scope.changeField = !$scope.changeField;
  }

  $scope.changeDurationFunc = function() {
    $scope.changeDuration = !$scope.changeDuration;
  }
  
  
  //$rootScope.division[$rootScope.currDivisionIndex] = $scope.divison;
}])
