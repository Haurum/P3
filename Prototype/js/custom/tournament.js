app.controller('TournamentController', ['$scope', '$rootScope', '$location', '$http', '$routeParams', '$uibModal', function ($scope, $rootScope, $location, $http, $routeParams, $uibModal) {

  $scope.getDivisions = function(){
    $http.get("http://localhost:50229/Tournament/Details?id=" +  $routeParams.tournamentId)
      .success(function(data)
      {
        $scope.EmFields = [];
        $scope.OmFields = [];
        $scope.FmFields = [];
        for (var i=0; i < data.Fields.length; i++)
        {
          if(data.Fields[i].fieldSize === 11)
          {
            $scope.EmFields.push(data.Fields[i]);
          }
          else if(data.Fields[i].fieldSize === 8)
          {
            $scope.OmFields.push(data.Fields[i]);
          }
          else
          {
            $scope.FmFields.push(data.Fields[i]);
          }
        }
        $scope.divisions = data.Divisions;
        $scope.tournament = data;
      }).error(function (err) {
        $scope.error = err;
      })
  }

  $scope.tournamentId = $routeParams.tournamentId;

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
    $location.url("tournament/" + $routeParams.tournamentId+ "/division/" + currDiv.Id);
  }

  /* Field start */
  $scope.EmField = "";
  $scope.OmField = "";
  $scope.FmField = "";
  $scope.newEm = false;
  $scope.newOm = false;
  $scope.newFm = false;

  $scope.submitField = function(fieldName, fieldSize) {
    $http.post($rootScope.apiUrl + "/Field/Create", { name: fieldName, size: fieldSize, tournamentId: $routeParams.tournamentId })
    .success(function(data){
        if(fieldSize === 11){
          $scope.createNewEmField();
        }
        else if(fieldSize === 8){
          $scope.createNewOmField();
        }
        else {
          $scope.createNewFmField();
        }
        $scope.getDivisions();
    }).error(function(err){
      $scope.createErr = err;
    })
    $scope.getDivisions();
  }
  
  $scope.removeField = function(Field) {
    $http.post("http://localhost:50229/Field/Delete", { id: Field.Id })
    .success(function(data){

    }).error(function(err){
      $scope.deleteErr = err;
    })
    $scope.getDivisions();
  }

  /* 11man */
  $scope.createNewEmField = function() {
    $scope.newEm = !$scope.newEm;
  }
  
  /* 8man */
 $scope.createNewOmField = function() {
    $scope.newOm = !$scope.newOm;
  }

  /* 5man */
  $scope.createNewFmField = function() {
    $scope.newFm = !$scope.newFm;
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

        $scope.getDivisions();
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

app.controller('CreateTournyController', ['$scope', '$rootScope', '$http', '$location', '$routeParams', 'FileUploader', function ($scope, $rootScope, $http, $location, $routeParams, FileUploader) {
  
  $scope.tournamentData =  {};

  var uploader = $scope.uploader = new FileUploader({
    url: $rootScope.apiUrl + '/Tournament/Create'
  });

  uploader.onBeforeUploadItem = function(item) {
    item.formData.push($scope.tournamentData);
  };
  uploader.onSuccessItem = function(fileItem, response, status, headers) {
    console.info('onSuccessItem', fileItem, response, status, headers);
    $location.path("tournament/" + response.id);
  };
  uploader.onErrorItem = function(fileItem, response, status, headers) {
    console.info('onErrorItem', fileItem, response, status, headers);
  };

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

  $scope.uploadTournament = function () 
  {
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
          $scope.tournamentData = {
            name: $scope.tournamentName,
            password: $scope.tournamentPassword,
            startTimes: $scope.startDateTimes,
            endTimes: $scope.endDateTimes
          }

          if (uploader.queue.length > 0)
          {
            uploader.queue[0].upload();
          }
          else
          {
            
            $http.post("http://localhost:50229/Tournament/Create/", $scope.tournamentData).success(function(Data)
            {
              if(Data.status === "error"){
                $scope.error = Data.message;
              } else {
                $location.path("tournament/" + Data.id);
              }
            }).error(function(err) 
            {
              $scope.error = "Kunne ikke uploade til serveren";
            });
          }
        }
      }  
    }  
  }
}]);

app.controller('EditTournamentController', ['$scope', '$rootScope', '$http', '$location', '$routeParams', function ($scope, $rootScope, $http, $location, $routeParams) {

  $http.get("http://localhost:50229/Tournament/Details?id=" + $routeParams.tournamentId).success(function(data){
    
    if(data.status === "success"){
    
      $scope.tournamentId = data.Id;
      $scope.tournamentName = data.Name;
      $scope.tournamentPassword = data.Password;
      $scope.dateArray = [];
      $scope.startTimes = [];
      $scope.endTimes = [];
      
      $scope.startDate = new Date(parseInt(data.TimeIntervals[0].StartTime.substr(6)));
      $scope.startDate.setHours(0);
      $scope.startDate.setSeconds(0);
      $scope.startDate.setMinutes(0);
      $scope.startDate.setMilliseconds(0);
      $scope.endDate = new Date(parseInt(data.TimeIntervals[data.TimeIntervals.length-1].EndTime.substr(6)));
      $scope.endDate.setHours(0);
      $scope.endDate.setSeconds(0);
      $scope.endDate.setMinutes(0);
      $scope.endDate.setMilliseconds(0);
      $scope.dateRange = ($scope.endDate - $scope.startDate) / (1000 * 60 * 60 * 24);
      
      for (var index = 0; index < data.TimeIntervals.length; index++) {
        var date = new Date($scope.startDate.getTime());
        date.setDate(date.getDate() + index);
        $scope.dateArray.push(date);
        $scope.startTimes.push(new Date(parseInt(data.TimeIntervals[index].StartTime.substr(6))));
        $scope.endTimes.push(new Date(parseInt(data.TimeIntervals[index].EndTime.substr(6))));
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
    }else{
      $scope.error = "Kunne ikke finde turneringen";
    }
  }).error(function(err){
    $scope.error = "Kunne ikke finde turneringen";
  });
  
}]);