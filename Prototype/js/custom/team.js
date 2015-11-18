app.controller('TeamDetailController', ['$scope', '$rootScope', '$location', '$http', function ($scope, $rootScope, $location, $http) {
  $scope.changeName = false;

  $scope.getTeamData = function() {
    $http.get($rootScope.apiUrl + "/Team/Details?id=" +  $routeParams.teamId)
    .success(function(data)
    {
      $scope.team = data;
    }).error(function(err) 
    {
      $scope.error = err;
    })
  }
  $scope.getTeamData();


}]);