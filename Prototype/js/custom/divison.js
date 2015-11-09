angular.module('tournyplanner').controller('DivisonDetailController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {
  $scope.changeField = false;
  $scope.changeDuration = false;
  $scope.newPool = false;
  $scope.newPoolName = "";
  $scope.index = $rootScope.currDivisionIndex;
  $scope.newPoolFunc = function() {
    $scope.newPool = !$scope.newPool;
  }
  
  
  $scope.addPool = function(name) {
    $scope.newPoolName = "";
    if (name != "")
    {
      $rootScope.divisions[$scope.index].Pool.push({ Name: name, Teams: [], IsOpen: false });
      $scope.newPoolFunc();
    }

  }
  
  $scope.remove = function() {
    $rootScope.divisions.splice($scope.index, 1);
    $location.path("/divisions");
  }

  $scope.removeTeam = function() {
    $rootScope.divisions[$scope.index].Pool[$scope.index].Teams.splice($scope.index, 1);
  }
  
  $scope.addTeamToPool = function(newTeamName, index) {
    $rootScope.divisions[$scope.index].Pool[index].Teams.push(newTeamName);
  }
  
  $scope.changeFieldFunc = function() {
    $scope.changeField = !$scope.changeField;
  }

  $scope.changeDurationFunc = function() {
    $scope.changeDuration = !$scope.changeDuration;
  }
  
  
  //$rootScope.division[$rootScope.currDivisionIndex] = $scope.divison;
}])
