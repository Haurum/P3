// PoolController is the controller for the pool.html page,
app.controller('PoolController', ['$scope', '$rootScope', '$location', '$http', '$routeParams', function ($scope, $rootScope, $location, $http, $routeParams) {
  $scope.changeName = false
  $scope.FavoriteFieldIds = [];
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
        for(var i = 0; i < $scope.pool.FavoriteFields.length; i++){
          $scope.FavoriteFieldIds.push($scope.pool.FavoriteFields[i].Id);
        }
        $scope.divisionFieldSize = data.FieldSize;
      } else {
        $scope.ErrorMessage = "Pulje kunne ikke læses";
      }
      
    }).error(function(err) 
    {
      $scope.error = err;
    })
  }

  $scope.getPoolData();
  
  $scope.EmField = [];
  $scope.OmField = [];
  $scope.FmField = [];

  // getFields is a get-request, which
  // returns a JSON object, containing the required information
  // for all the fields, and puts them into a fieldsize-fitted list.
  $scope.getFields = function() {
    $http.get($rootScope.apiUrl + "/Field/GetAllTournamentFields?tournamentId=" + $routeParams.tournamentId)
    .success(function(data){
      if(data.status === "success"){
        for (var i=0; i < data.Fields.length; i++)
        {
          if(data.Fields[i].fieldSize === 11)
          {
            $scope.EmField.push(data.Fields[i]);
          }
          else if(data.Fields[i].fieldSize === 8)
          {
            $scope.OmField.push(data.Fields[i]);
          }
          else
          {
            $scope.FmField.push(data.Fields[i]);
          }
        }
      } else {
        $scope.ErrorMessage = "Baner kunne ikke læses";
      } 
    }).error(function(err){
      $scope.error = err;
    })
  }

  $scope.getFields();
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
        $scope.ErrorMessage = "Kunne ikke oprette hold";
      }
    }).error(function(err){
     $scope.deleteErr = err;
    })   
    $scope.getPoolData();
  }

  // gotoTeamDetail is a function redirecting the user
  // to a specific teams html page.
  $scope.gotoTeamDetail = function(currTeam, index) {
    $rootScope.currTeamIndex = index;
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

  $scope.favFieldsIds = [];

  // setFavFieldFunc is a function, that adds a specific
  // user-chosen field to a pools favorite fields list,
  // through a post-request, given the pools Id, 
  // divisionId, pool name and the fields Id.
  $scope.setFavFieldFunc = function (favFieldsId) {
    $scope.favFieldsIds.push(favFieldsId);
    console.log($scope.favFieldsIds);
    $http.post($rootScope.apiUrl + "/Pool/Edit", { id: $routeParams.poolId, name: $scope.pool.Name, divisionId: $routeParams.divisionId, fieldIds: $scope.favFieldsIds})
    .success(function(data){
      if(data.status === "success"){
      } else {
        $scope.ErrorMessage = "Kunne ikke sætte foretrukne bane";
      }
    }).error(function(err){
      $scope.favFieldErr = err;
      console.log(err);
    })
  }

  // remove is the delete function for a pool, through
  // a post-request, with the parameter poolId. Returns
  // a JSON object containing a message: "success" or "error",
  // indicating wether the pool was deleted or not.
  $scope.remove = function() {
    $http.post($rootScope.apiUrl + "/Pool/Delete", { id: $routeParams.poolId })
    .success(function(data) {
      if(data.status === "success"){
        $location.path("/tournament/" + $routeParams.tournamentId + "/division/" + $routeParams.divisionId);
      } else {
        $scope.ErrorMessage = "Kunne ikke slætte pulje";
      }
    }).error(function(data) {
      $scope.deleteErr = data;
    })   
  }

  // removeTeam is the post-request to delete a team,
  // through the pools html page, with the parameter teamId.
  // This function returns a JSON object containing a message:
  // "success" or "error", indicating wether the team
  // was deleted or not.
  $scope.removeTeam = function(team) {
    $http.post($rootScope.apiUrl + "/Team/Delete", { id: team.Id })
    .success(function(data){
      if(data.status === "success"){
      } else {
        $scope.ErrorMessage = "Kunne ikke slætte hold";
      }
    }).error(function(data){
      $scope.deleteErr = data;
    })
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
    $http.post($rootScope.apiUrl + "/Pool/Edit", { name: newName, id: $routeParams.poolId, divisionId: $routeParams.divisionId, fieldIds: $scope.favFieldsIds})
    .success(function(data){
      if(data.status === "success"){
        console.log(data);
        $scope.pool.Name = data.newName;
        $scope.changePoolNameFunc();
        $scope.getPoolData();
      } else {
        $scope.ErrorMessage = "Kunne ikke skifte pulje-navn";
      }
    }).error(function(err){
      $scope.editErr = err;
    })
  }
}]);