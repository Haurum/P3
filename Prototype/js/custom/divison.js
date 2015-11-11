angular.module('tournyplanner').controller('DivisonController', ['$scope', '$rootScope', '$location', '$http', function ($scope, $rootScope, $location, $http) {
  $scope.changeField = false;
  $scope.changeDuration = false;
  $scope.changeFavField = false;
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
    $location.path("/tournament");
  }

  $scope.removePool = function() {
    $rootScope.divisions[$scope.index].Pool.splice($scope.index, 1);
    $location.path("/division");
  }
  
  $scope.changeFieldFunc = function() {
    $scope.changeField = !$scope.changeField;
  }

  $scope.changeDurationFunc = function() {
    $scope.changeDuration = !$scope.changeDuration;
  }

  $scope.gotoPool = function(currPool, index) {
    $rootScope.currPoolIndex = index;
    $location.url("/pool")
  }
  
  //$rootScope.division[$rootScope.currDivisionIndex] = $scope.divison;
}])
