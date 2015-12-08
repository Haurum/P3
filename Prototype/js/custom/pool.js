// PoolController is the controller for the pool.html page,
app.controller('PoolController', ['$scope', '$rootScope', '$location', '$http', '$routeParams', '$window', function ($scope, $rootScope, $location, $http, $routeParams, $window) {
  $scope.changeName = false
  $scope.orderByField = 'Number';
  $scope.reverseSort = false;
  
  // getPoolData is a get-request, which
  // returns a JSON object, containing the required information
  // for a specific pool.
  $scope.getPoolData = function(){
    $http.get($rootScope.apiUrl + "/Pool/Details?id=" +  $routeParams.poolId)
    .success(function(data)
    {
      if(data.status === "success"){
        $scope.pool = data;
        for(var i = 0; i < $scope.pool.Matches.length; i++)
        {
          $scope.pool.Matches[i].StartTime = new Date(parseInt($scope.pool.Matches[i].StartTime.substr(6)));
        }
        $scope.divisionFieldSize = data.FieldSize;
      } else {
        $scope.error = "Pulje kunne ikke læses";
      }     
    }).error(function(err) 
    {
      $scope.error = err;
    })
  }
  $scope.getPoolData();

  $scope.newTeamName="";  
  // addTeamToPool is a post-request which sends
  // the poolId and a team name to the c# TeamController.Create
  // method, returning a JSON object containing a message: "success" or "error",
  // indicating wether the team was created or not.
  $scope.addTeamToPool = function(name, index) {
    $http.post($rootScope.apiUrl + "/Team/Create", { name: name, poolId: $routeParams.poolId })
    .success(function(data) {
      if(data.status === "success"){
        $scope.getPoolData();
      } else {
        $scope.error = "Kunne ikke oprette hold";
      }
    }).error(function(err){
     $scope.deleteErr = err;
    })   
    $scope.getPoolData();
  }

  // gotoTeamDetail is a function redirecting the user
  // to a specific teams html page.
  $scope.gotoTeamDetail = function(currTeam) {
    $location.url($location.url() + "/team/" + currTeam.Id);
  }

  // gotoTournament is a function redirecting the user
  // to a specific tournaments html page.
  $scope.gotoTournament = function() {
    $location.url("/tournament/" + $routeParams.tournamentId);
  }

  // gotoDivision is a function redirecting the user
  // to a specific divisions html page.
  $scope.gotoDivision = function() {
    $location.url("/tournament/" + $routeParams.tournamentId + "/division/" + $routeParams.divisionId);
  }

  // remove is the delete function for a pool, through
  // a post-request, with the parameter poolId. Returns
  // a JSON object containing a message: "success" or "error",
  // indicating wether the pool was deleted or not.
  $scope.remove = function() {
    var deletePool = $window.confirm('Er du sikker på du vil slette puljen?');

    if(deletePool){
      $http.post($rootScope.apiUrl + "/Pool/Delete", { id: $routeParams.poolId })
      .success(function(data) {
        if(data.status === "success"){
          $location.path("/tournament/" + $routeParams.tournamentId + "/division/" + $routeParams.divisionId);
        }
        else {
          $scope.error = "Kunne ikke slætte pulje";
        }
      }).error(function(data) {
        $scope.deleteErr = data;
      })
    }   
  }

  // changePoolNameFunc is a function called by the
  // changeNewPoolNameFunc, which changes a boolean
  // "changeName" to the opposite of what is was
  // when the function is called. This boolean
  // maps wether a button should be shown or not.
  $scope.changePoolNameFunc = function() {
    $scope.changeName = !$scope.changeName;
  }

  // changeNewPoolNameFunc is the function to change a pool name,
  // through a post-request, with the parameters: new pool name,
  // poolId, divisionId and favorite fields ids.
  $scope.changeNewPoolNameFunc = function(newName) {
    $http.post($rootScope.apiUrl + "/Pool/Edit", { name: newName, id: $routeParams.poolId, divisionId: $routeParams.divisionId})
    .success(function(data){
      if(data.status === "success"){
        $scope.pool.Name = data.newName;
        $scope.changePoolNameFunc();
        $scope.getPoolData();
      } else {
        $scope.error = "Kunne ikke skifte pulje-navn";
      }
    }).error(function(err){
      $scope.editErr = err;
    })
  }
}]);