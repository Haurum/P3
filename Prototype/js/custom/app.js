var app = angular.module('tournyplanner', ['ngRoute', 'ui.bootstrap', 'angularFileUpload', 'angular-loading-bar']);

// routeProvider routes the user to the right html page and provides the controller
// for that specific page, given the input from the user (buttons etc).
app.config(['$routeProvider', function ($routeProvider) {
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
    when('/tutorial', {
      templateUrl: 'templates/tutorial.html',
      controller: 'HomeController'
    }).
    otherwise({
      redirectTo: '/'
    });
}]);

app.config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
  cfpLoadingBarProvider.includeSpinner = true;
  cfpLoadingBarProvider.parentSelector = '#navbar';
}]);

app.run(function ($rootScope, $http, $routeParams, $q) {
  $rootScope.apiUrl = "http://localhost:50229";

  $rootScope.scheduler = function (tournamentID) {
    console.log("Sletter nuværende kampprogram");
    $http.get($rootScope.apiUrl + "/ScheduleManager/DeleteSchedule?tournamentID=" + tournamentID)
      .success(function (deleteData) {
        console.log("Kampprogram slettet")

        console.log("Validere turnering");
        $http.get($rootScope.apiUrl + "/Validator/IsScheduleReady?tournamentID=" + tournamentID)
          .success(function (validateData) {
            if (validateData.status === "success") {
              console.log("Generer gruppespil");
              $http.get($rootScope.apiUrl + "/MatchGeneration/GenerateGroupStage?tournamentID=" + tournamentID)
                .success(function (generateGSData) {
                  if (generateGSData.status === "success") {
                    console.log("Generer slutspils hold");
                    $http.get($rootScope.apiUrl + "/MatchGeneration/GenerateFinalsTeams?tournamentID=" + tournamentID)
                      .success(function (generateFTData) {
                        if (generateFTData.status === "success") {
                          console.log("Generer slutspils kampe");
                          $http.get($rootScope.apiUrl + "/MatchGeneration/GenerateFinalsMatches?tournamentID=" + tournamentID)
                            .success(function (generateFMData) {
                              if (generateFMData.status === "success") {
                                console.log("starter planlægning");
                                $http.get($rootScope.apiUrl + "/ScheduleManager/Schedule?tournamentID=" + tournamentID + "&fs=" + 11)
                                  .success(function (eScheduleData) {
                                    if (eScheduleData.status === "success") {
                                      console.log("success");

                                    }
                                    else {

                                    }
                                  }).error(function () {

                                  })
                              }
                              else {

                              }
                            }).error(function () {

                            })
                        }
                        else {

                        }
                      }).error(function () {

                      })
                  }
                  else {

                  }
                }).error(function () {

                })
            }
            else {

            }
          }).error(function (err) {

          })
      }).error(function () {

      })
  }

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
  $scope.getId = function (password) {
    $http.post("http://localhost:50229/Tournament/IdFromPass", { password: password })
      .success(function (passwordData) {
        if (passwordData.Id != 0) {
          $scope.error = false;
          $location.path("tournament/" + passwordData.Id);
        }
        else {
          $scope.error = true;
        }
      }).error(function (err) {
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