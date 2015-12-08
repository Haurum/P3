var app = angular.module('tournyplanner', ['ngRoute', 'ui.bootstrap', 'angularFileUpload', 'angular-loading-bar']);

// routeProvider routes the user to the right html page and provides the controller
// for that specific page, given the input from the user (buttons etc).
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
     when('/schedule', {
      templateUrl: 'templates/schedule.html',
      controller: 'ScheduleController'
    }).   
    otherwise({
      redirectTo: '/'
    });
}]);

app.config(['cfpLoadingBarProvider', function(cfpLoadingBarProvider) {
  cfpLoadingBarProvider.includeSpinner = true;
  cfpLoadingBarProvider.parentSelector = '#navbar';
}]);

app.run(function($rootScope) {
  $rootScope.divisions = [];
  $rootScope.EmFields = [];
  $rootScope.OmFields = [];
  $rootScope.FmFields = [];
  $rootScope.Tournament = {};
  $rootScope.apiUrl = "http://localhost:50229";
});

// HomeController is the controller for the home.html page,
// where the "log-in" or "create new tournament" options are available.
app.controller('HomeController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
  
  $scope.password = "";

  $scope.errMsg = function () {
    $scope.error = !$scope.error;
  }

  // getId is the "log-in" to a tournament, which needs the password parameter,
  // to redirect the user to the specific tournament.
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

app.filter('jsonDate', ['$filter', function ($filter) {
    return function (input) {
        return (input) ? $filter('date')(parseInt(input.substr(6)), "yyyy-MM-dd HH:mm") : '';
    };
}]);
app.filter('jsonOnlyDate', ['$filter', function ($filter) {
    return function (input) {
        return (input) ? $filter('date')(parseInt(input.substr(6)), "yyyy-MM-dd") : '';
    };
}]);
app.filter('jsonOnlyTime', ['$filter', function ($filter) {
    return function (input) {
        return (input) ? $filter('date')(parseInt(input.substr(6)), "HH:mm") : '';
    };
}]);
app.filter('jsonOnlyHour', ['$filter', function ($filter) {
    return function (input) {
        return (input) ? $filter('date')(parseInt(input.substr(6)), "HH") : '';
    };
}]);