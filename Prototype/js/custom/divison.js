angular.module('tournyplanner').controller('DivisonDetailController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {
  $scope.changeField = false;
  $scope.changeDuration = false;
  $scope.changeFavField = false;
  $scope.newPool = false;
  $scope.newPoolName = "";
  $scope.index = $rootScope.currDivisionIndex;

  /* Post & Get requests */
  /*$rootScope.Team.Id = 1;

  $http.get("http://localhost:50229/Teams?id=" +  $rootScope.Team.Id)
  .success(function(teamData)
  {
    $scope.Teams = teamData;
  }).error(function(err) 
  {
    $scope.error = err;
  })

  $http.get("http://localhost:50229/Teams", params { teamName: "teamName" } )
  .success(function(teamData)
  {
    $scope.Teams = teamData;
  }).error(function(err) 
  {
    $scope.error = err;
  })

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

  $scope.removePool = function() {
    $rootScope.divisions[$scope.index].Pool.splice($scope.index, 1);
    $location.path("/divisions/detail");
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

  $scope.changeFavFieldFunc = function() {
    $scope.changeFavField = !$scope.changeFavField;
  }

  $scope.gotoPool = function(currPool, index) {
    $rootScope.currPoolIndex = index;
    $location.url("/divisions/detail/pools")
  }

  $scope.gotoTeamDetail = function(currTeam, index) {
    $rootScope.currTeamIndex = index;
    $location.url("/TeamDetail");
  }

  
  //$rootScope.division[$rootScope.currDivisionIndex] = $scope.divison;
}])
