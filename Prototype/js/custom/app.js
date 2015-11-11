angular.module('tournyplanner', ['ngRoute', 'ui.bootstrap'])

.config(['$routeProvider', function($routeProvider) {
  $routeProvider.
    when('/', {
      templateUrl: 'templates/home.html',
      controller: 'HomeController'
    }).
    when('/addTourny', {
      templateUrl: 'templates/OpretNT.html',
      controller: 'CreateTournyController'
    }).
    when('/divisions/:id', {
      templateUrl: 'templates/tournament-divs.html',
      controller: 'DivisonController'
    }).
    when('/divisions/detail', {
      templateUrl: 'templates/tournament-divs-detail.html',
      controller: 'DivisonDetailController'
    }).
    when('/divisions/detail/pools', {
      templateUrl: 'templates/pools.html',
      controller: 'DivisonDetailController'
    }).
    when('/addFields', {
      templateUrl: 'templates/baner.html',
      controller: 'CreateFieldsController'
    }).
    when('/DivisionMatchOverview', {
      templateUrl: 'templates/DivisionMatchOverview.html',
      controller: 'DivisionMatchOverviewController'
    }).
    when('/TeamMatchOverview', {
      templateUrl: 'templates/TeamMatchOverview.html',
      controller: 'TeamMatchOverviewController'
    }).
    when('/PoolMatchOverview', {
      templateUrl: 'templates/PoolMatchOverview.html',
      controller: 'PoolMatchOverviewController'
    }).
    when('/TournamentSchedule', {
      templateUrl: 'templates/TournamentSchedule.html',
      controller: 'TournamentScheduleController'
    }).
    when('/FinalStage', {
      templateUrl: 'templates/finalStage.html',
      controller: 'FinalStageController'
    }).     
    when('/TeamDetail', {
      templateUrl: 'templates/TeamDetail.html',
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
    $http.post("http://localhost:50229/Tournament/", {params: { password: password }})
    .success(function(passwordData)
    {
      $location.url("#/divisions/" + passwordData.Id);
    }).error(function(err) 
    {
      $scope.error = err;
    });
    }

}])

.controller('CreateTournyController', ['$scope', '$rootScope', function ($scope, $rootScope) {
  $scope.helper = true;
  $scope.withExcel = function() {
    if ($scope.helper)
    {
      $rootScope.divisions = [{ MatchDuration: "30", FieldSize: "11-mands", Name: "U17 Drenge A", Pool: [{ Name: "Pulje 1", Teams: ["Hjørring FC", "Aab", "Thisted FC", "Klinkby B"], IsOpen: false}, { Name: "Pulje 2", Teams: ["Randers", "FCK", "Hobro", "Lemvig"], IsOpen: false}]}, 
      { MatchDuration: "30", FieldSize: "11-mands", Name: "U17 Drenge B", Pool: [{ Name: "Pulje 1", Teams: ["FC Midtjylland", "Ab", "Thy FC", "Klinkerne"], IsOpen: false}, { Name: "Pulje 2", Teams: ["Anholdt", "FLY", "Holstebro", "Lemvig"], IsOpen: false}]}, 
      { MatchDuration: "30", FieldSize: "8-mands", Name: "U13 Pige A", Pool: [{ Name: "Pulje 1", Teams: ["Holstebro", "Klinkerne", "Thisted", "Aab"], IsOpen: false}, { Name: "Pulje 2", Teams: ["Chang", "Freja", "HIF", "FCLV"], IsOpen: false}]}, 
      { MatchDuration: "30", FieldSize: "8-mands", Name: "U13 Pige B", Pool: [{ Name: "Pulje 1", Teams: ["Aab B", "Hold A", "Fredericia", "Vejle B"], IsOpen: false}, { Name: "Pulje 2", Teams: ["Vejle A", "Kolding", "KFUM Aalborg", "KFUM Aarhus"], IsOpen: false}]}, 
      { MatchDuration: "30", FieldSize: "5-mands", Name: "U8 Drenge", Pool: [{ Name: "Pulje 1", Teams: ["Aab", "Hirsthals", "Thy FC", "Klinkby B"], IsOpen: false}, { Name: "Pulje 2", Teams: ["Aars", "KFUM Aalborg", "HIF", "FCL"], IsOpen: false}]}];
      $rootScope.EmFields = ["Bane 1", "Bane 2", "Bane 5", "Bane 6"];
      $rootScope.OmFields = ["Bane 3A", "Bane 3B", "Bane 4A", "Bane 4B"];
      $rootScope.FmFields = ["Bane 7A", "Bane 7B", "Bane 7C", "Bane 7D"];
    }
    $scope.helper = !$scope.helper;
  }
  if ($scope.withExcel)
  {
    
  }
  $scope.isCollapsed = true;
}])

.controller('DivisionMatchOverviewController', ['$scope', function ($scope) {
  $scope.isCollapsed = true;
}])

.controller('PoolMatchOverviewController', ['$scope', function ($scope) {
  $scope.isCollapsed = true;
}])

.controller('TeamMatchOverviewController', ['$scope', function ($scope) {
  $scope.isCollapsed = true;
}])


.controller('TournamentScheduleController', ['$scope', function ($scope) {
  $scope.isCollapsed = true;
}])

.controller('FinalStageController', ['$scope', function ($scope) {
}])

.controller('CreateFieldsController', ['$scope', '$rootScope', '$http', function ($scope, $rootScope, $http) {

  /* Post & Get requests */ 
 /* $rootScope.Field.Id = 1;

  $http.get("http://localhost:50229/Field?id=" +  $rootScope.Field.Id)
  .success(function(fieldData)
  {
    $scope.Field = fieldData;
  }).error(function(err) 
  {
    $scope.error = err;
  })

  $http.post("http://localhost:50229/Field", params { hejhej: "hejgej", hej2: "htdyfg" })
  .success(function(fieldData)
  {
    $scope.Field = fieldData;
  }).error(function(err) 
  {
    $scope.error = err;
  }) */

  $scope.newFieldName = "";

  $scope.newEm = false;

  $scope.newOm = false;

  $scope.newFm = false; 

  /* 11mands */
  $scope.createNewEmField = function() {
    $scope.newEm = !$scope.newEm;
  }
  $scope.submitEmField = function(newName) {
    $rootScope.EmFields.push(newName);
    $scope.newFieldName = "";
    $scope.createNewEmField();
  }
  
  $scope.removeEmField = function(index) {
    $rootScope.EmFields.splice(index, 1);
  }

  /* 8mands */
 $scope.createNewOmField = function() {
    $scope.newOm = !$scope.newOm;
  }
  $scope.submitOmField = function(newName) {
    $rootScope.OmFields.push(newName);
    $scope.newFieldName = "";
    $scope.createNewOmField();
  }
  
  $scope.removeOmField = function(index) {
    $rootScope.OmFields.splice(index, 1);
  }  

  /* 5mands */
  $scope.createNewFmField = function() {
    $scope.newFm = !$scope.newFm;
  }
  $scope.submitFmField = function(newName) {
    $rootScope.FmFields.push(newName);
    $scope.newFieldName = "";
    $scope.createNewFmField();
  }
  
  $scope.removeFmField = function(index) {
    $rootScope.FmFields.splice(index, 1);
  }  

}])

.controller('DivisonController', ['$scope', '$rootScope', '$location', '$http', function ($scope, $rootScope, $location, $http) {
  $rootScope.Tournament.password = "superkode";

  $http.get("http://localhost:50229/Tournament/DetailsFromPass/?password=" +  $rootScope.Tournament.password)
  .success(function(data)
  {
    $scope.divisions = data.Divisions;
  }).error(function(err) 
  {
    $scope.error = err;
  })
  
  $scope.newDivName = "";
  
  $scope.new = false;
  
  $scope.createNew = function() {
    $scope.new = !$scope.new;
  }
  $scope.submit = function(newName) {
    $rootScope.divisions.push({ MatchDuration: "30", FieldSize: "11-mands", Name: newName, Pool: []});
    $scope.newDivName = "";
    $scope.createNew();
  }
  
  $scope.gotoDivison = function(currDiv, index) {
    $rootScope.currDivisionIndex = index;
    $location.url("/divisions/detail");
  }

}])


/* DATE PICKER START! */

.controller('DatepickerDemoCtrl', function ($scope) {
  $scope.today = function() {
    $scope.dt = new Date();
  };
  $scope.today();

  $scope.clear = function () {
    $scope.dt = null;
  };

  $scope.toggleMin = function() {
    $scope.minDate = $scope.minDate ? null : new Date();
  };
  $scope.toggleMin();
  $scope.maxDate = new Date(2020, 5, 22);

  $scope.open = function($event) {
    $scope.status.opened = true;
  };

  $scope.setDate = function(year, month, day) {
    $scope.dt = new Date(year, month, day);
  };

  $scope.dateOptions = {
    formatYear: 'yy',
    startingDay: 1
  };

  $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
  $scope.format = $scope.formats[0];

  $scope.status = {
    opened: false
  };

  var tomorrow = new Date();
  tomorrow.setDate(tomorrow.getDate() + 1);
  var afterTomorrow = new Date();
  afterTomorrow.setDate(tomorrow.getDate() + 2);
  $scope.events =
    [
      {
        date: tomorrow,
        status: 'full'
      },
      {
        date: afterTomorrow,
        status: 'partially'
      }
    ];

  $scope.getDayClass = function(date, mode) {
    if (mode === 'day') {
      var dayToCheck = new Date(date).setHours(0,0,0,0);

      for (var i=0;i<$scope.events.length;i++){
        var currentDay = new Date($scope.events[i].date).setHours(0,0,0,0);

        if (dayToCheck === currentDay) {
          return $scope.events[i].status;
        }
      }
    }

    return '';
  };
})

/* DATE PICKER END! */ 

.controller('TeamDetailController', ['$scope', function ($scope) {
  /* Den rigtige controller er i app.js pt. */
}])