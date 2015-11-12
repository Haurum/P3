app.controller('PoolController', ['$scope', '$rootScope', '$location', '$http', function ($scope, $rootScope, $location, $http) {
/*
  $rootScope.Pool.Id = 1;

  $http.get("http://localhost:50229/Pool?id=" +  $rootScope.Pool.Id)
  .success(function(poolData)
  {
    $scope.Pool = poolData;
  }).error(function(err) 
  {
    $scope.error = err;
  })

  $http.get("http://localhost:50229/Pool", params { poolName: "poolName" } )
  .success(function(poolData)
  {
    $scope.Pool = poolData;
  }).error(function(err) 
  {
    $scope.error = err;
  })*/

  $scope.removeTeam = function() {
    $rootScope.divisions[$scope.index].Pool[$scope.index].Teams.splice($scope.index, 1);
  }

  $scope.addTeamToPool = function(newTeamName, index) {
    $rootScope.divisions[$scope.index].Pool[index].Teams.push(newTeamName);
  }

  $scope.gotoTeamDetail = function(currTeam, index) {
    $rootScope.currTeamIndex = index;
    $location.url("/team");
  }

  $scope.changeFavFieldFunc = function() {
    $scope.changeFavField = !$scope.changeFavField;
  }

}]);