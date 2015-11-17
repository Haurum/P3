var app = angular.module('tournyplanner', ['ngRoute', 'ui.bootstrap', 'angularFileUpload']);

app.config(['$routeProvider', function($routeProvider) {
  $routeProvider.
    when('/', {
      templateUrl: 'templates/home.html',
      controller: 'HomeController'
    }).
    when('/addTournament', {
      templateUrl: 'templates/addTournament.html',
      controller: 'CreateTournyController'
    }).
    when('/tournament/:tournamentId/edit', {
      templateUrl: 'templates/editTournament.html',
      controller: 'EditTournamentController'
    }).
    when('/tournament/:tournamentId', {
      templateUrl: 'templates/tournament.html',
      controller: 'TournamentController'
    }).
    when('/tournament/:tournamentId/division/:divisionId', {
      templateUrl: 'templates/division.html',
      controller: 'DivisionController'
    }).
    when('/tournament/:tournamentId/division/:divisionId/pool/:poolId', {
      templateUrl: 'templates/pool.html',
      controller: 'PoolController'
    }).
    when('/tournament/:tournamentId/field/:fieldId', {
      templateUrl: 'templates/field.html',
      controller: 'TournamentController'
    }).
    when('/tournament/:tournamentId/division/:divisionId/pool/:poolId/team/:teamId', {
      templateUrl: 'templates/team.html',
      controller: 'TeamDetailController'
    }).   
    otherwise({
      redirectTo: '/'
    });
}]);

app.run(function($rootScope) {
  $rootScope.divisions = [];
  $rootScope.EmFields = [];
  $rootScope.OmFields = [];
  $rootScope.FmFields = [];
  $rootScope.Tournament = {};
  $rootScope.apiUrl = "http://localhost:50229";
});

app.controller('HomeController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
  $scope.password = "";

  $scope.getId = function(password)
  {
    $http.post("http://localhost:50229/Tournament/IdFromPass", { password: password })
    .success(function(passwordData)
    {
      if (passwordData.Id != 0)
      {
        $scope.error = false;
        $location.path("tournament/" + passwordData.Id);
      }
      else{
        $scope.error = true;
      }
    }).error(function(err) 
    {
      $scope.error = err;
    });
  }
}]);
