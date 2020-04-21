$(document).ready(
    function () {
        var currentCost = $('.js-seat-container')[0].dataset.currentCost;
        var currentTimeSlotId =
            $('.js-seat-container')[0].dataset.currentTimeslotId;

        //Get the contents from the script block
        var source = document.querySelector(".js-selected-seat-template").innerHTML;
        //Compile that baby into a template
        var template = Handlebars.compile(source);

        var selectedSeats = {
            addedSeats: [],
            sum: 0
        };
        $(".js-seat-container").on("click",
            ".js-seat-selector",
            function(e) {
                var targetElem = e.currentTarget;
                var dataSet = targetElem.dataset;
                var newSeat = {
                    row: dataSet.seatRow,
                    seat: dataSet.seatCol,
                    elem: targetElem
                };
                var existingSeatIndex = -1;
                for (var i = 0; i < selectedSeats.addedSeats.length; i++) {
                    var currentSeat = selectedSeats.addedSeats[i];
                    if (currentSeat.row === newSeat.row &&
                        currentSeat.seat === newSeat.seat) {
                        existingSeatIndex = i;
                        break;
                    }
                }
                if (existingSeatIndex !== -1) {
                    selectedSeats.addedSeats.splice(existingSeatIndex, 1);
                } else {
                    selectedSeats.addedSeats.push(newSeat);
                }
                selectedSeats.sum = currentCost * selectedSeats.addedSeats.length;
                var resultString = template(selectedSeats);
                $(".js-seat-result-container").html(resultString);

            });
        $(".js-seat-container").on("click",
            ".js-reserve-seats",
            function() {
                sendSeatsToServer("reserve");
            });
        $(".js-seat-container").on("click",
            ".js-buy-seats",
            function () {
                sendSeatsToServer("buy");
            });
        function sendSeatsToServer(status) {
            var resultModel = {
                seatsRequest: selectedSeats,
                selectedStatus: status,
                timeslotId: currentTimeSlotId
            };
            $.ajax({
                url: "/tickets/processRequest",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(resultModel)
            }).done(function() {
                for (var i = 0; i < selectedSeats.addedSeats.length; i++) {
                    var currentSeat = selectedSeats.addedSeats[i].elem;
                    if (status === 'reserve') {
                        currentSeat.parentNode.classList.add('is-reserved');
                    } else {
                        currentSeat.parentNode.classList.add('is-sold');
                    }
                    currentSeat.checked = false;
                    currentSeat.disabled = true;
                }
                selectedSeats.addedSeats = [];
                $(".js-seat-result-container").html("");
            }).fail(function() { alert("Failed") });
        }

    }
);