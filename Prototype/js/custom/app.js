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
    when('/tournament/:id', {
      templateUrl: 'templates/tournament.html',
      controller: 'TournamentController'
    }).
    when('/division/:id', {
      templateUrl: 'templates/division.html',
      controller: 'DivisonController'
    }).
    when('/pool/:id', {
      templateUrl: 'templates/pool.html',
      controller: 'PoolController'
    }).
    when('/field', {
      templateUrl: 'templates/field.html',
      controller: 'CreateFieldsController'
    }).
    when('/team/:id', {
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