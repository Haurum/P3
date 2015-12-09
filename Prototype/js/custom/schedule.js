app.controller('ScheduleController', ['$scope', '$rootScope', '$location', '$http', '$routeParams', '$filter', function ($scope, $rootScope, $location, $http, $routeParams, $filter) {
  $scope.chosenDay = 0;
  $scope.incDay = function() {
    if ($scope.chosenDay < $scope.tournament.TimeIntervals.length-1)
      $scope.chosenDay++;
  }
  $scope.decDay = function() {
    if ($scope.chosenDay > 0)
      $scope.chosenDay--;
  }
  $scope.getData = function(){
<<<<<<< HEAD
    $http.get($rootScope.apiUrl + "/Field/GetAllTournamentFields?tournamentId=" + 17).success(function(data) {
=======
    $http.get("http://localhost:50229/Field/GetAllTournamentFields?tournamentId=" + 14).success(function(data) {
>>>>>>> 36b31fca2f44734d31909101d019d67b61333b8f
      $scope.tournament = data;
      $scope.days = [];
      for (var i=0; i < $scope.tournament.Fields.length; i++) {
        for (var j=0; j < $scope.tournament.Fields[i].matches.length; j++) {
          $scope.tournament.Fields[i].matches[j].StartTime = $filter('jsonOnlyTime')($scope.tournament.Fields[i].matches[j].StartTime);
          $scope.tournament.Fields[i].matches[j].EndTime = $filter('jsonOnlyTime')($scope.tournament.Fields[i].matches[j].EndTime);
          $scope.tournament.Fields[i].matches[j].Date = $filter('jsonOnlyDate')($scope.tournament.Fields[i].matches[j].Date);
        }
      }
      for (var i=0; i < $scope.tournament.TimeIntervals.length; i++) {
        $scope.tournament.TimeIntervals[i].StartTime = $filter('jsonOnlyTime')($scope.tournament.TimeIntervals[i].StartTime);
        $scope.tournament.TimeIntervals[i].EndTime = $filter('jsonOnlyTime')($scope.tournament.TimeIntervals[i].EndTime);
        $scope.tournament.TimeIntervals[i].Date = $filter('jsonOnlyDate')($scope.tournament.TimeIntervals[i].Date);
      }
    })
  }   
  $scope.getData();    
}]);