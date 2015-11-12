var app = angular.module('tournyplanner', ['ngRoute', 'ui.bootstrap'])

.config(['$routeProvider', function($routeProvider) {
  $routeProvider.
    when('/', {
      templateUrl: 'templates/home.html',
      controller: 'HomeController'
    }).
    when('/addTournament', {
      templateUrl: 'templates/addTournament.html',
      controller: 'CreateTournyController'
    }).
    when('/tournament/:tournamentId', {
      templateUrl: 'templates/tournament.html',
      controller: 'TournamentController'
    }).
    when('tournament/:tournamentId/division/:divisionId', {
      templateUrl: 'templates/division.html',
      controller: 'DivisonController'
    }).
    when('tournament/:tournamentId/division/:divisionId/pool/:poolId', {
      templateUrl: 'templates/pool.html',
      controller: 'PoolController'
    }).
    when('/tournament/:tournamentId/field', {
      templateUrl: 'templates/field.html',
      controller: 'CreateFieldsController'
    }).
    when('tournament/:tournamentId/division/:divisionId/pool/:poolId/team/:teamId', {
      templateUrl: 'templates/team.html',
      controller: 'TeamDetailController'
    }).   
    otherwise({
      redirectTo: '/'
    });
}])

.run(function($rootScope) {
  $rootScope.divisions = [];
  $rootScope.EmFields = [];
  $rootScope.OmFields = [];
  $rootScope.FmFields = [];
  $rootScope.Tournament = {};
})

.controller('HomeController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
  $scope.password = "";

  $scope.getId = function(password)
  {
    $http.post("http://localhost:50229/Tournament/IdFromPass", { password: password })
    .success(function(passwordData)
    {
      if (passwordData.Id != 0)
      {
        $scope.error = false;
        console.log($location.url);
        $location.path("tournament/" + passwordData.Id);
        console.log($location.url);
      }
      else{
        $scope.error = true;
      }
    }).error(function(err) 
    {
      $scope.error = err;
    });
  }
}])