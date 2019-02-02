using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PA2_Toni_Gashi1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Calendar.MinDate = DateTime.Today.Date;
            List<string> lines = File.ReadAllLines(filePath).ToList();

            foreach (string line in lines)
            {
                string[] entries = line.Split(',');
                Passenger newPassenger = new Passenger(Convert.ToInt16(entries[0]), entries[1], entries[2], entries[3], entries[4], entries[5]);
                Passengers.Add(newPassenger);
                cmbList.Items.Add(newPassenger.PassengerName);
                flightNumber++;
            }
            clnTakeOff.MinDate= DateTime.Today.Date; 
            List<string> lines2 = File.ReadAllLines(filePath2).ToList();
            txtAllFlights.Text = "Time".PadRight(8) + "Date".PadRight(22) + "From".PadRight(16) + "To".PadRight(12) + "Flight\n";
            foreach(string line in lines2)
            {
                string[] entries = line.Split(',');
                Flight newFlight = new Flight(Convert.ToString( entries[0]), entries[1], entries[2], entries[3],entries[4], Convert.ToString(entries[5]),Convert.ToInt16( entries[6]),Convert.ToInt16( entries[7]),Convert.ToString(entries[8]),Convert.ToInt16( entries[9]),entries[10]);
                Flights.Add(newFlight);
                if (newFlight.NumberOfWays == 2)
                {
                    cmbInToolStripFlights.Items.Add($"{newFlight.TakeOffPlace}-{newFlight.Destination}");
                }
                else
                {
                    cmbOneWay.Items.Add($"{newFlight.TakeOffPlace}-{newFlight.Destination}");
                }
                txtAllFlights.Text += newFlight.FlightTime.PadRight(8) + newFlight.TakeOffDate.PadRight(18) + newFlight.TakeOffPlace.PadRight(16) + newFlight.Destination.PadRight(12) + newFlight.FlightID+"\n";
            }
            }
        bool bookingIsValid=true;
        string nrOfWays = "";
        string route = "";
        bool validFlight= true;
        public string filePath = @"C:\Users\User\Desktop\PA2\files\Passenger.txt";
        public string filePath2 = @"C:\Users\User\Desktop\PA2\files\Flights.txt";
        List<Passenger> Passengers = new List<Passenger>();
        List<Flight> Flights = new List<Flight>();
        int flightNumber=0;
        bool freeSeat = true;
        List<string> output = new List<string>();
        List<string> output2 = new List<string>();
        string date;
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
          
        }

        private void cmbInToolStripFlights_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblHidden.Visible = true;
            lblHidden.Text = "Two-way ticket to ";
            lblHidden.Text += cmbInToolStripFlights.Text;
            nrOfWays = "Two-way";
            route = cmbInToolStripFlights.Text;
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            freeSeat=true;
            if(cmbSeatNumber.Text!="" && cmbSeatLetter.Text!="" && txtFirstName.Text!=""&&txtLastName.Text!="" && lblHidden.Visible==true)
            {
                for(int i=0; i<Passengers.Count;i++)
                {
                    if (cmbSeatNumber.Text + " " + cmbSeatLetter.Text == Passengers[i].SeatNumber && Convert.ToString( Calendar.Value) == Passengers[i].Date && nrOfWays==Passengers[i].NumberOfWays && route==Passengers[i].FlightRoute)
                    {
                        MessageBox.Show("This specific seat is taken, we encourage you to change the seat number or if you cant find a seat please select another date.", "Problem");
                        freeSeat = false;
                        bookingIsValid = false;
                    }
                   
                    
                }
                
                if (freeSeat == true)
                {
                        MessageBox.Show("The seat is free to book.", "Automated Reply");
                        bookingIsValid = true;
                    
                    
                }
                
            }
            else
            {
                if(lblHidden.Visible == false)
                {
                    MessageBox.Show("Please select a route.");
                }
                else
                    MessageBox.Show("Please enter the empty entrie(s).", "Missing Entrie(s)");
            }
        }

        private void btnBooking_Click(object sender, EventArgs e)
        {
            if(txtLastName.Text!="" && txtFirstName.Text!="")
            {

                if(bookingIsValid==true)
                {
                    if (lblHidden.Visible == true)
                    {
                        MessageBox.Show("Booking Complete", "Confirmed");
                        flightNumber++;
                        Passenger nextPassenger = new Passenger(flightNumber, route, nrOfWays, date, txtFirstName.Text + " " + txtLastName.Text, cmbSeatNumber.Text + " " + cmbSeatLetter.Text);
                        
                        Passengers.Add(nextPassenger);
                        output.Clear();
                        foreach(var passe in Passengers)
                        {
                            output.Add($"{passe.TicketNumber},{passe.FlightRoute},{passe.NumberOfWays},{passe.Date},{passe.PassengerName},{passe.SeatNumber}");
                            
                        }
                        File.WriteAllLines(filePath,output);
                        cmbList.Items.Add(nextPassenger.PassengerName);
                        txtFirstName.Text = "";
                        txtLastName.Text = "";
                        cmbSeatNumber.Text = "";
                        cmbSeatLetter.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Please click 'passengers' then choose a flight option and your route", "Route Selection Mistake");
                    }
                }
                else
                {
                    MessageBox.Show("Please make sure to check if the seat is free. If it isn't please select a valid seat.", "Recheck");
                }
            }
        }

        private void comboBoxInMenuStripOneWay_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblHidden.Visible = true;
            lblHidden.Text = "One-way ticket to ";
            lblHidden.Text += cmbOneWay.Text;
            nrOfWays = "One-way";
            route = cmbOneWay.Text;
        }

        private void Calendar_ValueChanged(object sender, EventArgs e)
        {
            date =Convert.ToString( Calendar.Value);
        }

        private void ticketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(1);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Application.Exit();
        }

        private void cmbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int temp = cmbList.SelectedIndex;
            MessageBox.Show($"Ticket number {Passengers[temp].TicketNumber}\nFlight:{Passengers[temp].FlightRoute}\n{Passengers[temp].NumberOfWays} ticket\nDate:{Passengers[temp].Date}\nName: {Passengers[temp].PassengerName}\nSeat: {Passengers[temp].SeatNumber}\nEtc. ... ");
        }

        private void btnNewFlight_Click(object sender, EventArgs e)
        {
            if (txtAirplaneType.Text != "" && txtDestination.Text != "" && txtEatingOnBoard.Text != "" && txtFlightID.Text != "" && txtNumberOfPassengers.Text != "" && txtNumberOfRows.Text != "" && txtStopovers.Text != "" && txtTakeOffPlace.Text != "")
            {
                foreach (var fly in Flights)//all the file
                {
                    if (fly.FlightID == txtFlightID.Text)//already exists
                    {
                        MessageBox.Show("The flight allready exists, please make sure you input the right flight.", "Flight Exists");
                        validFlight = false;
                    }
                }

                if (validFlight == true)//not necessary just showing I know how to do it
                {
                    int a = 0, b = 0, c = 0, d = 0;
                    try
                    {
                        a = Convert.ToInt32(txtNumberOfPassengers);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        d++;
                    }
                    try
                    {
                        b = Convert.ToInt32(txtNumberOfRows);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        d++;
                    }
                    try
                    {
                        c = Convert.ToInt32(txtNumberOfWays);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        d++;
                    }
                    if (d == 0)
                    {

                        MessageBox.Show("Added to File", "Done");
                        Flight newFlight = new Flight(Convert.ToString(DateTime.Now.TimeOfDay.Hours + ":" + DateTime.Now.TimeOfDay.Minutes), txtFlightID.Text, txtTakeOffPlace.Text, txtDestination.Text, txtStopovers.Text, txtAirplaneType.Text, a, b, txtEatingOnBoard.Text, c, Convert.ToString(clnTakeOff.SelectionStart.Month + "/" + clnTakeOff.SelectionStart.Day + "/" + clnTakeOff.SelectionStart.Year));
                        Flights.Add(newFlight);
                        if (newFlight.NumberOfWays == 2)
                        {
                            cmbInToolStripFlights.Items.Add($"{newFlight.TakeOffPlace}-{newFlight.Destination}");
                        }
                        else
                        {
                            cmbOneWay.Items.Add($"{newFlight.TakeOffPlace}-{newFlight.Destination}");
                        }
                        output2.Clear();
                        foreach (var temp in Flights)
                        {
                            output2.Add($"{temp.FlightTime},{temp.FlightID},{temp.TakeOffPlace},{temp.Destination},{temp.Stopovers},{temp.AirplaneType},{temp.NumberOfPassengers},{temp.NumberOfRows},{temp.EatingOnBoard},{temp.NumberOfWays},{temp.TakeOffDate}");

                        }
                        File.WriteAllLines(filePath2, output2);
                        txtAllFlights.Text +=
                        txtFlightID.Text = "";
                        txtNumberOfPassengers.Text = "";
                        txtNumberOfRows.Text = "";
                        txtNumberOfWays.Text = "";
                        txtEatingOnBoard.Text = "";
                        txtStopovers.Text = "";
                        txtTakeOffPlace.Text = "";
                        txtDestination.Text = "";
                        txtAirplaneType.Text = "";
                    }
                }

            }
            
            else
            {
                MessageBox.Show("Please fill all the empty spots.");
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtFlightID.Text = "";
            txtNumberOfPassengers.Text = "";
            txtNumberOfRows.Text = "";
            txtNumberOfWays.Text = "";
            txtEatingOnBoard.Text = "";
            txtStopovers.Text = "";
            txtTakeOffPlace.Text = "";
            txtDestination.Text = "";
            txtAirplaneType.Text = "";
            txtFlightID.Focus();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            output2.Clear();
            File.WriteAllLines(filePath2, output2);
            txtAllFlights.Text = "Time".PadRight(8) + "Date".PadRight(22) + "From".PadRight(16) + "To".PadRight(12) + "Flight\n";
        }
    }
}
