app.controller('ScheduleController', ['$scope', '$rootScope', '$location', '$http', '$routeParams', function ($scope, $rootScope, $location, $http, $routeParams) {
  
  $scope.getData = function(){
    $http.get("http://localhost:50229/Tournament/Details?id=" + 1)
    .success(function(data)
    {
      $scope.tournament = data;
    }).error(function (err) {
      $scope.error = err;
    })
  }
  
  $scope.getData();
  

}]);