app.controller('DivisonController', ['$scope', '$rootScope', '$location', '$http', '$routeParams', function ($scope, $rootScope, $location, $http, $routeParams) {
  $scope.changeField = false;
  $scope.changeDuration = false;
  $scope.changeFavField = false;
  $scope.newPool = false;
  $scope.newPoolName = "";

  $scope.getDivisionData = function() {
    $http.get($rootScope.apiUrl + "/Divison/Details?id=" +  $routeParams.divisionId)
    .success(function(data)
    {
      $scope.division = data;
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
    $http.post($rootScope.apiUrl + "/Pool/Create", { id: $routeParams.poolId })
    .success(function(data) {
      $scope.newPoolFunc();
    }).error(function(data){
     $scope.deleteErr = data;
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

  $scope.changeDurationFunc = function() {
    $scope.changeDuration = !$scope.changeDuration;
  }

  $scope.gotoPool = function(currPool, index) {
    $rootScope.currPoolIndex = index;
    $location.url($location.url() + "/pool/" + currPool.Id);
  }
  
  //$rootScope.division[$rootScope.currDivisionIndex] = $scope.divison;
}]);
