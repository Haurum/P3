app.controller('PoolController', ['$scope', '$rootScope', '$location', '$http', '$routeParams', function ($scope, $rootScope, $location, $http, $routeParams) {
  $scope.changeName = false
  $scope.FavoriteFieldIds = [];
  $scope.orderByField = 'Number';
  $scope.reverseSort = false;
  
  $scope.getPoolData = function(){
    $http.get($rootScope.apiUrl + "/Pool/Details?id=" +  $routeParams.poolId)
    .success(function(data)
    {
      $scope.pool = data;
      for(var i = 0; i < $scope.pool.FavoriteFields.length; i++){
        $scope.FavoriteFieldIds.push($scope.pool.FavoriteFields[i].Id);
      }
      $scope.divisionFieldSize = data.FieldSize;
    }).error(function(err) 
    {
      $scope.error = err;
    })
  }
  $scope.getPoolData();
  
  $scope.EmField = [];
  $scope.OmField = [];
  $scope.FmField = [];

  $scope.getFields = function() {
    $http.get($rootScope.apiUrl + "/Field/GetAllTournamentFields?tournamentId=" + $routeParams.tournamentId)
    .success(function(data){
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
    }).error(function(err){
      $scope.error = err;
    })
  }

  $scope.getFields();

  $scope.getPoolData();

  $scope.addTeamToPool = function(name, index) {
    $http.post($rootScope.apiUrl + "/Team/Create", { name: name, poolId: $routeParams.poolId })
    .success(function(data) {
      $scope.getPoolData();
    }).error(function(err){
     $scope.deleteErr = err;
    })   
    $scope.getPoolData();
  }

  $scope.gotoTeamDetail = function(currTeam, index) {
    $rootScope.currTeamIndex = index;
    $location.url($location.url() + "/team/" + currTeam.Id);
  }

  $scope.gotoTournament = function() {
    $location.url("/tournament/" + $routeParams.tournamentId);
  }

  $scope.gotoDivision = function() {
    $location.url("/tournament/" + $routeParams.tournamentId + "/division/" + $routeParams.divisionId);
  }

  $scope.favFieldsIds = [];
  $scope.setFavFieldFunc = function (favFieldsId) {
    $scope.favFieldsIds.push(favFieldsId);
    console.log($scope.favFieldsIds);
    $http.post($rootScope.apiUrl + "/Pool/Edit", { id: $routeParams.poolId, name: $scope.pool.Name, divisionId: $routeParams.divisionId, fieldIds: $scope.favFieldsIds})
    .success(function(data){

    }).error(function(err){
      $scope.favFieldErr = err;
      console.log(err);
    })
  }

  $scope.remove = function() {
    $http.post($rootScope.apiUrl + "/Pool/Delete", { id: $routeParams.poolId })
    .success(function(data) {
      $location.path("/tournament/" + $routeParams.tournamentId + "/division/" + $routeParams.divisionId);
    }).error(function(data) {
      $scope.deleteErr = data;
    })   
  }

  $scope.removeTeam = function(team) {
    $http.post($rootScope.apiUrl + "/Team/Delete", { id: team.Id })
    .success(function(data){

    }).error(function(data){
      $scope.deleteErr = data;
    })
  }

  $scope.changePoolNameFunc = function() {
    $scope.changeName = !$scope.changeName;
  }

  $scope.changeNewPoolNameFunc = function(newName) {
    $http.post($rootScope.apiUrl + "/Pool/Edit", { name: newName, id: $routeParams.poolId, divisionId: $routeParams.divisionId, fieldIds: $scope.favFieldsIds})
    .success(function(data){
      console.log(data);
      $scope.pool.Name = data.newName;
      $scope.changePoolNameFunc();
      $scope.getPoolData();
    }).error(function(err){
      $scope.editErr = err;
    })
  }

}]);