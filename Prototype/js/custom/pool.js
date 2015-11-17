app.controller('PoolController', ['$scope', '$rootScope', '$location', '$http', '$routeParams', function ($scope, $rootScope, $location, $http, $routeParams) {
  $scope.getPoolData = function(){
    $http.get($rootScope.apiUrl + "/Pool/Details?id=" +  $routeParams.poolId)
    .success(function(data)
    {
      $scope.pool = data;
      $scope.divisionFieldSize = data.FieldSize;
      console.log(data.FieldSize);
      console.log($scope.pool.FieldSize);
    }).error(function(err) 
    {
      $scope.error = err;
    })
  }
  $scope.EmField = [];
  $scope.OmField = [];
  $scope.FmField = [];

  $scope.getFields = function() {
    $http.get($rootScope.apiUrl + "/Field/GetAllTournamentFields?tournamentId=" + $routeParams.tournamentId)
    .success(function(data){
              console.log(data);
      for (var i=0; i < data.Fields.length; i++)
        {
          if(data.Fields[i].fieldSize === 11)
          {
            $scope.EmField.push(data.Fields[i]);
            console.log(EmField);
          }
          else if(data.Fields[i].fieldSize === 8)
          {
            $scope.OmField.push(data.Fields[i]);
          }
          else
          {
            $scope.FmFields.push(data.Fields[i]);
          }
        }
        console.log($scope.EmField);
    }).error(function(err){
      $scope.error = err;
    })
  }

  $scope.getFields();

  $scope.getPoolData();

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

  $scope.setFavFieldFunc = function (favFieldsId) {
    $scope.favFieldsIds = [];
    $scope.favFieldsIds.push(favFieldsId);
    $http.post($rootScope + "/Pool/Edit", { id: $routeParams.poolId, name: pool.Name, divisionId: $routeParams.divisionId, fieldsIds: $scope.favFieldsIds})
    .success(function(data){
      console.log(data);
    }).error(function(err){
      $scope.favFieldErr = err;
    })
  }

  $scope.remove = function(pool) {
    $http.post($rootScope.apiUrl + "/Pool/Delete", { id: $routeParams.poolId })
    .success(function(data) {
      $location.path("/tournament/" + $routeParams.tournamentId + "/division/" + $routeParams.divisionId);
    }).error(function(data) {
      $scope.deleteErr = data;
    })   
  }

}]);