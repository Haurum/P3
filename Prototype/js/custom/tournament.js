app.controller('TournamentController', ['$scope', '$rootScope', '$location', '$http', '$routeParams', '$uibModal', function ($scope, $rootScope, $location, $http, $routeParams, $uibModal) {
  $rootScope.Tournament.password = "";


  $scope.getDivisions = function(){
    $http.get("http://localhost:50229/Tournament/Details?id=" +  $routeParams.tournamentId)
      .success(function(data)
      {
        $scope.divisions = data.Divisions;
      }).error(function (err) {
        $scope.error = err;
      })
  }

  $scope.getDivisions();

  $scope.newDivName = "";
  $scope.chooseField = "";
  $scope.newMatchDuration = "";
  
  $scope.new = false;

  $scope.createNew = function () {
    $scope.new = !$scope.new;
  }

  /* Modal start */
  $scope.animationsEnabled = true;

  $scope.open = function (size) {

    var uibModalInstance = $uibModal.open({
      animation: $scope.animationsEnabled,
      templateUrl: 'myModalContent.html',
      controller: 'ModalInstanceCtrl',
      size: size,
      /*resolve: {
        items: function () {
          return $scope.items;
        }
      }*/
    });

    uibModalInstance.result.then(function () {
      $scope.getDivisions();
        $scope.createNew();
    }, function () {
      $log.info('Modal dismissed at: ' + new Date());
    });

  };

  $scope.toggleAnimation = function () {
    $scope.animationsEnabled = !$scope.animationsEnabled;
  };
  /* Modal end */


  $scope.gotoDivison = function (currDiv, index) {
    $rootScope.currDivisionIndex = index;
    console.log("tournament/" + $routeParams.tournamentId+ "/division/" + currDiv.Id);
    $location.url("tournament/" + $routeParams.tournamentId+ "/division/" + currDiv.Id);
  }

  /* Field start */
  $scope.EmField = "";
  $scope.OmField = "";
  $scope.FmField = "";
  $scope.newEm = false;
  $scope.newOm = false;
  $scope.newFm = false;
  
  $scope.getFields = function(){
    $http.get("http://localhost:50229/Field/Details?id=" +  $routeParams.fieldId)
      .success(function(data)
      {
        $scope.fields = data;

        /*for(int i=0; i <= data.length; i++)
        {
          if(data.FieldSize === 11)
          {
            $scope.EmField = data.Field;
          }
          if(data.FieldSize === 8)
          {
            $scope.OmField = data.Field;
          }
          else
          {
            $scope.FmField = data.Field;
          }
        }*/
      }).error(function (err) {
        $scope.error = err;
      })
  }
  $scope.getFields();

  /* 11man */
  $scope.createNewEmField = function() {
    $scope.newEm = !$scope.newEm;
  }
  $scope.submitField = function(EmField) {
    



    $scope.Emfield = "";
    $scope.createNewEmField();
  }
  
  $scope.removeEmField = function(index) {
    $rootScope.EmField.splice(index, 1);
  }

  /* 8man */
 $scope.createNewOmField = function() {
    $scope.newOm = !$scope.newOm;
  }
  $scope.submitOmField = function(OmField) {
    $rootScope.OmFields.push(OmField);
    $scope.OmField = "";
    $scope.createNewOmField();
  }
  
  $scope.removeOmField = function(index) {
    $rootScope.OmFields.splice(index, 1);
  }  

  /* 5man */
  $scope.createNewFmField = function() {
    $scope.newFm = !$scope.newFm;
  }
  $scope.submitFmField = function(FmField) {
    $rootScope.FmFields.push(FmField);
    $scope.FmField = "";
    $scope.createNewFmField();
  }
  
  $scope.removeFmField = function(index) {
    $rootScope.FmFields.splice(index, 1);
  }  

  /* Field end */

}]);

app.controller('ModalInstanceCtrl', ['$scope', '$uibModalInstance', '$http', '$routeParams', function ($scope, $uibModalInstance, $http, $routeParams) {

    $scope.newDivName = "";
    $scope.newMatchDuration = "";
    $scope.chooseField = "";
  $scope.submitNewDiv = function(newDivName, newMatchDuration, chooseField) {
    $http.post("http://localhost:50229/Division/Create", { Name: newDivName, MatchDuration: newMatchDuration, FieldSize: chooseField, tournamentId: $routeParams.tournamentId })
      .success(function(data){
        
        $uibModalInstance.close();
        $scope.newDivName = "";
        $scope.newMatchDuration = "";
        $scope.chooseField = "";
        
      }).error(function(data){
        $scope.newDivError = data;
      })
    } 

  $scope.ok = function () {
    $uibModalInstance.close();
  };

  $scope.cancel = function () {
    $uibModalInstance.dismiss('cancel');
  };
}]);

app.controller('CreateTournyController', ['$scope', '$rootScope', '$http', '$location', '$routeParams', function ($scope, $rootScope, $http, $location, $routeParams) {
   
  /* DATE PICKER START */
  $scope.dateRange = 0;
  $scope.today = function () {
    $scope.startDate = new Date();
    $scope.startDate.setHours(0);
    $scope.startDate.setSeconds(0);
    $scope.startDate.setMinutes(0);
    $scope.startDate.setMilliseconds(0);
    $scope.endDate = new Date();
    $scope.endDate.setHours(0);
    $scope.endDate.setSeconds(0);
    $scope.endDate.setMinutes(0);
    $scope.endDate.setMilliseconds(0);
  };
  $scope.today();

  $scope.dateArray = [$scope.startDate];
  $scope.startTimes = [$scope.startDate];
  $scope.endTimes = [$scope.startDate];

  $scope.toggleMin = function () {
    $scope.minDate = $scope.minDate ? null : new Date();
  };
  $scope.toggleMin();
  $scope.maxDate = new Date(2020, 5, 22);
  $scope.openEndDate = function ($event) {
    $scope.statusEndDate.opened = true;
  };

  $scope.isChanged = function () {
    $scope.dateRange = ($scope.endDate - $scope.startDate) / (1000 * 60 * 60 * 24);
    $scope.dateArray = [];
    $scope.startTimes = [];
    $scope.endTimes = [];
    for (var i = 0; i <= $scope.dateRange; i++) {
      var date = new Date($scope.startDate.getTime());
      date.setDate(date.getDate() + i);
      $scope.dateArray.push(date);
      $scope.startTimes.push(date);
      $scope.endTimes.push(date);
    }
  }

  $scope.openStartDate = function ($event) {
    $scope.statusStartDate.opened = true;
  };
  $scope.dateOptions = {
    formatYear: 'yy',
    startingDay: 1
  };
  $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
  $scope.format = $scope.formats[0];
  $scope.statusStartDate = {
    opened: false
  };
  $scope.statusEndDate = {
    opened: false
  };
  /* DATE PICKER END */





  $scope.uploadTournament = function () {
    if (!$scope.tournamentName || !$scope.tournamentPassword){
      $scope.error = "Navn eller kode ikke sat";
    }else{
      $scope.startDateTimes = [];
      $scope.endDateTimes = [];
      $scope.error = false;
      for (var index = 0; index <= $scope.dateRange; index++) {
        $scope.startDateTimes[index] = $scope.startTimes[index].toISOString();
        $scope.endDateTimes[index] = $scope.endTimes[index].toISOString();        
        
      }
      
      if($scope.startDateTimes.length-1 !== $scope.dateRange && $scope.endDateTimes.length-1 !== $scope.dateRange){
        console.log($scope.startDateTimes.length);
        console.log($scope.dateRange);
        $scope.error = "Fejl i start eller slut tidspunkt for en af dagene";
           
      }else{
        for(var i = 0; i <= $scope.dateRange; i++){
          if($scope.startDateTimes[i] >= $scope.endDateTimes[i]){
            $scope.error = "alle slut tidspunkter skal være senere end start tidspunkter";
          }
        }
        if(!$scope.error){
          var tournamentData = {
            name: $scope.tournamentName,
            password: $scope.tournamentPassword,
            startTimes: $scope.startDateTimes,
            endTimes: $scope.endDateTimes
          }
      
          $http.post("http://localhost:50229/Tournament/Create/", tournamentData).success(function(Data)
          {
            if(Data.status === "error"){
              $scope.error = Data.message;
            }else{
              $location.path("tournament/" + Data.Id);
            }
          }).error(function(err) 
          {
            $scope.error = "Kunne ikke uploade til serveren";
          });
        }
      }  
    }   
  }
}]);

app.controller('EditTournamentController', ['$scope', '$rootScope', '$http', '$location', '$routeParams', function ($scope, $rootScope, $http, $location, $routeParams) {
  
  $http.get("http://localhost:50229/Tournament/Details?id=" + $routeParams.tournamentId).success(function(data){
    
    console.log(data);
    $scope.tournamentId = data.Id;
    $scope.tournamentName = data.Name;
    $scope.tournamentPassword = data.Password;
    $scope.dateArray = [];
    $scope.startTimes = [];
    $scope.endTimes = [];
    
    $scope.dateRange = 0;
    $scope.startDate = new Date(data.TimeIntervals[0].StartTime);
    console.log($scope.startDate);
    $scope.startDate.setHours(0);
    $scope.startDate.setSeconds(0);
    $scope.startDate.setMinutes(0);
    $scope.startDate.setMilliseconds(0);
    $scope.endDate = new Date(data.TimeIntervals[data.TimeIntervals.length-1].StartTime);
    $scope.endDate.setHours(0);
    $scope.endDate.setSeconds(0);
    $scope.endDate.setMinutes(0);
    $scope.endDate.setMilliseconds(0);
    
    for (var index = 0; index < data.TimeIntervals.length; index++) {
      var date = new Date($scope.startDate.getTime());
      date.setDate(date.getDate() + index);
      $scope.dateArray.push(date);
      $scope.startTimes.push(new Date(data.TimeIntervals[index].StartTime));
      $scope.endTimes.push(new Date(data.TimeIntervals[index].EndTIme))
    }
    $scope.toggleMin = function () {
      $scope.minDate = $scope.minDate ? null : new Date();
    };
    $scope.toggleMin();
    $scope.maxDate = new Date(2020, 5, 22);
    $scope.openEndDate = function ($event) {
      $scope.statusEndDate.opened = true;
    };
  
    $scope.isChanged = function () {
      $scope.dateRange = ($scope.endDate - $scope.startDate) / (1000 * 60 * 60 * 24);
      $scope.dateArray = [];
      $scope.startTimes = [];
      $scope.endTimes = [];
      for (var i = 0; i <= $scope.dateRange; i++) {
        var date = new Date($scope.startDate.getTime());
        date.setDate(date.getDate() + i);
        $scope.dateArray.push(date);
        $scope.startTimes.push(date);
        $scope.endTimes.push(date);
      }
    }
  
    $scope.openStartDate = function ($event) {
      $scope.statusStartDate.opened = true;
    };
    $scope.dateOptions = {
      formatYear: 'yy',
      startingDay: 1
    };
    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
    $scope.format = $scope.formats[0];
    $scope.statusStartDate = {
      opened: false
    };
    $scope.statusEndDate = {
      opened: false
    };
    
    $scope.uploadTournament = function () {
      if (!$scope.tournamentName || !$scope.tournamentPassword){
        $scope.error = "Navn eller kode ikke sat";
      }else{
        $scope.startDateTimes = [];
        $scope.endDateTimes = [];
        $scope.error = false;
        for (var index = 0; index <= $scope.dateRange; index++) {
          $scope.startDateTimes[index] = $scope.startTimes[index].toISOString();
          $scope.endDateTimes[index] = $scope.endTimes[index].toISOString();        
          
        }
        
        if($scope.startDateTimes.length-1 !== $scope.dateRange && $scope.endDateTimes.length-1 !== $scope.dateRange){
          console.log($scope.startDateTimes.length);
          console.log($scope.dateRange);
          $scope.error = "Fejl i start eller slut tidspunkt for en af dagene";
            
        }else{
          for(var i = 0; i <= $scope.dateRange; i++){
            if($scope.startDateTimes[i] >= $scope.endDateTimes[i]){
              $scope.error = "alle slut tidspunkter skal være senere end start tidspunkter";
            }
          }
          if(!$scope.error){
            var tournamentData = {
              id: $scope.tournamentId,
              name: $scope.tournamentName,
              password: $scope.tournamentPassword,
              startTimes: $scope.startDateTimes,
              endTimes: $scope.endDateTimes
            }
        
            $http.post("http://localhost:50229/Tournament/Edit/", tournamentData).success(function(Data)
            {
              if(Data.status === "error"){
                $scope.error = Data.message;
              }else{
                $location.path("tournament/" + Data.Id);
              }
            }).error(function(err) 
            {
              $scope.error = "Kunne ikke uploade til serveren";
            });
          }
        }  
      }   
    }
  }).error(function(err){
    
  });
  
}]);