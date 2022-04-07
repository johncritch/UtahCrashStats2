// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';

// Pie Chart Example

var ctx = document.getElementById("severity");
var myPieChart = new Chart(ctx, {
  type: 'doughnut',
  data: {
    labels: ["No Injuries", "Possible Injuries", "Minor Injuries", "Serious Injuries", "Fatal Injuries"],
    datasets: [{
      data: [178471, 44229, 24087, 4682, 968],
        backgroundColor: ['#1cc88a', '#36b9cc', '#4e73df', '#f6c23e', '#e74a3b'],
        hoverBackgroundColor: ['#17a673', '#2c9faf', '#2e59d9', '#dda20a', '#be2617'],
      hoverBorderColor: "rgba(234, 236, 244, 1)",
    }],
  },
  options: {
    maintainAspectRatio: false,
    tooltips: {
      backgroundColor: "rgb(255,255,255)",
      bodyFontColor: "#858796",
      borderColor: '#dddfeb',
      borderWidth: 1,
      xPadding: 15,
      yPadding: 15,
      displayColors: false,
      caretPadding: 10,
    },
    legend: {
      display: false
    },
    cutoutPercentage: 80,
  },
});
