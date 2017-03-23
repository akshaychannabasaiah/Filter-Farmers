using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace FilterFarmers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<FarmerProfile> filterList = new ObservableCollection<FarmerProfile>();
        XElement visitorProfile = null;
        List<XElement> records = null;
        XDocument doc = null;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            listBoxFarmerProfile.ItemsSource = filterList;
            listBoxFarmerProfile.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("MatchCount", System.ComponentModel.ListSortDirection.Descending));

            LoadXML();

        }

        private void LoadXML()
        {
            doc = XDocument.Load("data.xml");
            records = doc.Root.Elements().ToList();
            cmbMachinery.Items.Add("No Filter");
            cmbAnimals.Items.Add("No Filter");
            cmbFacility.Items.Add("No Filter");
            cmbGrain.Items.Add("No Filter");
            cmbLocation.Items.Add("No Filter");
            cmbProject.Items.Add("No Filter");
            foreach (var rec in records)
            {
                try
                {
                    foreach (var ele in rec.Element("Machinery").Elements())
                    {
                        if (!cmbMachinery.Items.Contains(ele.Value) && ele.Value != "")
                            cmbMachinery.Items.Add(ele.Value);
                    }
                }
                catch { }
                try
                {
                    foreach (var ele in rec.Element("Animals").Elements())
                    {
                        if (!cmbAnimals.Items.Contains(ele.Name.LocalName) && ele.Name.LocalName != "")
                            cmbAnimals.Items.Add(ele.Name.LocalName);
                    }
                }
                catch { }
                try
                {
                    foreach (var ele in rec.Element("Facility").Elements())
                    {
                        if (!cmbFacility.Items.Contains(ele.Name.LocalName) && ele.Name.LocalName != "")
                            cmbFacility.Items.Add(ele.Name.LocalName);
                    }
                }
                catch { }
                try
                {
                    if (!cmbLocation.Items.Contains(rec.Element("Location").Element("Country").Value) && rec.Element("Location").Element("Country").Value != "")
                        cmbLocation.Items.Add(rec.Element("Location").Element("Country").Value);
                }
                catch { }
                try
                {
                    foreach (var ele in rec.Element("Project").Elements())
                    {
                        if (!cmbProject.Items.Contains(ele.Name.LocalName) && ele.Name.LocalName != "")
                            cmbProject.Items.Add(ele.Name.LocalName);
                    }
                }
                catch { }
                try
                {
                    foreach (var ele in rec.Element("size_of_farm").Elements().Where(d => d.Name.LocalName != "Total"))
                    {
                        if (!cmbGrain.Items.Contains(ele.Name.LocalName) && ele.Name.LocalName != "")
                            cmbGrain.Items.Add(ele.Name.LocalName);
                    }
                }
                catch { }
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadListBox();
            scrollFilterView.Visibility = Visibility.Collapsed;
            gridHomePage.Visibility = Visibility.Visible;
        }

        private void LoadListBox()
        {
            filterList.Clear();
            if (visitorProfile == null)
                visitorProfile = records[0];
            foreach (var rec in records)
            {
                FarmerProfile f = new FarmerProfile();
                f.FirstName = rec.Element("Contact_information").Element("FirstName").Value;
                f.LastName = rec.Element("Contact_information").Element("LastName").Value;
                f.Phone = rec.Element("Contact_information").Element("ContactNo").Value;
                f.Email = rec.Element("Contact_information").Element("Email").Value;

                f.FarmSize = int.Parse(rec.Element("size_of_farm").Element("Total").Value);
                int farmSizeVisitor = int.Parse(visitorProfile.Element("size_of_farm").Element("Total").Value);
                if (farmSizeVisitor == f.FarmSize || (f.FarmSize - 100 <= farmSizeVisitor && farmSizeVisitor <= f.FarmSize + 100))
                {
                    f.SizeImage = new BitmapImage(new Uri(@"pack://application:,,,/"
                                                    + Assembly.GetExecutingAssembly().GetName().Name
                                                    + ";component/"
                                                    + "Images/size_Y.png", UriKind.Absolute));
                    f.matchCount++;

                }
                else
                {
                    f.SizeImage = new BitmapImage(new Uri(@"pack://application:,,,/"
                                                    + Assembly.GetExecutingAssembly().GetName().Name
                                                    + ";component/"
                                                    + "Images/size_N.png", UriKind.Absolute));
                }

                string state = "";
                try
                {
                    state = rec.Element("Location").Element("State").Value;
                }
                catch { }
                string country = rec.Element("Location").Element("Country").Value;
                f.Country = country;
                f.State = state;

                string stateVisitor = visitorProfile.Element("Location").Element("State").Value;
                string countryVisitor = visitorProfile.Element("Location").Element("Country").Value;
                if (state == stateVisitor && country == countryVisitor)
                {
                    f.LocationImage = new BitmapImage(new Uri(@"pack://application:,,,/"
                                                    + Assembly.GetExecutingAssembly().GetName().Name
                                                    + ";component/"
                                                    + "Images/Location_Y.png", UriKind.Absolute));
                    f.matchCount++;
                }
                else
                {
                    f.LocationImage = new BitmapImage(new Uri(@"pack://application:,,,/"
                                                    + Assembly.GetExecutingAssembly().GetName().Name
                                                    + ";component/"
                                                    + "Images/Location_N.png", UriKind.Absolute));
                }

                List<string> animals = new List<string>();
                try
                {
                    foreach (var ele in rec.Element("Animals").Elements())
                    {
                        animals.Add(ele.Name.LocalName);
                    }
                    foreach (var ele in visitorProfile.Element("Animals").Elements())
                    {
                        if (animals.Contains(ele.Name.LocalName, StringComparer.InvariantCultureIgnoreCase))
                        {
                            f.AnimalsImage = new BitmapImage(new Uri(@"pack://application:,,,/"
                                                            + Assembly.GetExecutingAssembly().GetName().Name
                                                            + ";component/"
                                                            + "Images/Animals_Y.png", UriKind.Absolute));
                            f.matchCount++;
                        }

                    }
                }
                catch
                { }
                f.Animals = animals;
                if (f.AnimalsImage == null)
                {
                    f.AnimalsImage = new BitmapImage(new Uri(@"pack://application:,,,/"
                                                    + Assembly.GetExecutingAssembly().GetName().Name
                                                    + ";component/"
                                                    + "Images/Animals_N.png", UriKind.Absolute));
                }


                List<string> machinery = new List<string>();
                try
                {
                    foreach (var ele in rec.Element("Machinery").Elements())
                    {
                        machinery.Add(ele.Value);
                    }
                    foreach (var ele in visitorProfile.Element("Machinery").Elements())
                    {
                        if (machinery.Contains(ele.Value, StringComparer.InvariantCultureIgnoreCase))
                        {
                            f.MachineryImage = new BitmapImage(new Uri(@"pack://application:,,,/"
                                                            + Assembly.GetExecutingAssembly().GetName().Name
                                                            + ";component/"
                                                            + "Images/Machinery_Y.png", UriKind.Absolute));
                            f.matchCount++;
                        }
                    }
                }
                catch { }
                f.Machinery = machinery;
                if (f.MachineryImage == null)
                {
                    f.MachineryImage = new BitmapImage(new Uri(@"pack://application:,,,/"
                                                    + Assembly.GetExecutingAssembly().GetName().Name
                                                    + ";component/"
                                                    + "Images/Machinery_N.png", UriKind.Absolute));
                }


                List<string> facility = new List<string>();
                try
                {
                    foreach (var ele in rec.Element("Facility").Elements())
                    {
                        facility.Add(ele.Name.LocalName);
                    }
                    foreach (var ele in visitorProfile.Element("Facility").Elements())
                    {
                        if (facility.Contains(ele.Name.LocalName, StringComparer.InvariantCultureIgnoreCase))
                        {
                            f.FacilityImage = new BitmapImage(new Uri(@"pack://application:,,,/"
                                                            + Assembly.GetExecutingAssembly().GetName().Name
                                                            + ";component/"
                                                            + "Images/Facility_Y.png", UriKind.Absolute));
                            f.matchCount++;
                        }
                    }
                }
                catch { }
                f.Facility = facility;
                if (f.FacilityImage == null)
                {
                    f.FacilityImage = new BitmapImage(new Uri(@"pack://application:,,,/"
                                                    + Assembly.GetExecutingAssembly().GetName().Name
                                                    + ";component/"
                                                    + "Images/Facility_N.png", UriKind.Absolute));
                }

                List<string> project = new List<string>();
                try
                {
                    foreach (var ele in rec.Element("Project").Elements())
                    {
                        project.Add(ele.Name.LocalName);
                    }
                    foreach (var ele in visitorProfile.Element("Project").Elements())
                    {
                        if (project.Contains(ele.Name.LocalName, StringComparer.InvariantCultureIgnoreCase))
                        {
                            f.ProjectImage = new BitmapImage(new Uri(@"pack://application:,,,/"
                                                            + Assembly.GetExecutingAssembly().GetName().Name
                                                            + ";component/"
                                                            + "Images/Project_Y.png", UriKind.Absolute));
                            f.matchCount++;
                        }
                    }
                }
                catch { }
                f.Project = project;
                if (f.ProjectImage == null)
                {
                    f.ProjectImage = new BitmapImage(new Uri(@"pack://application:,,,/"
                                                    + Assembly.GetExecutingAssembly().GetName().Name
                                                    + ";component/"
                                                    + "Images/Project_N.png", UriKind.Absolute));
                }

                List<string> grain = new List<string>();
                try
                {
                    foreach (var ele in rec.Element("size_of_farm").Elements().Where(d => d.Name.LocalName != "Total"))
                    {
                        grain.Add(ele.Name.LocalName);
                    }
                    foreach (var ele in visitorProfile.Element("size_of_farm").Elements().Where(d => d.Name.LocalName != "Total"))
                    {
                        if (grain.Contains(ele.Name.LocalName, StringComparer.InvariantCultureIgnoreCase))
                        {
                            f.GrainImage = new BitmapImage(new Uri(@"pack://application:,,,/"
                                                            + Assembly.GetExecutingAssembly().GetName().Name
                                                            + ";component/"
                                                            + "Images/Grain_Y.png", UriKind.Absolute));
                            f.matchCount++;
                        }
                    }
                }
                catch { }
                f.Grain = grain;
                if (f.GrainImage == null)
                {
                    f.GrainImage = new BitmapImage(new Uri(@"pack://application:,,,/"
                                                    + Assembly.GetExecutingAssembly().GetName().Name
                                                    + ";component/"
                                                    + "Images/Grain_N.png", UriKind.Absolute));
                }

                filterList.Add(f);

            }
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listBoxFarmerProfile.Items.Filter = delegate (object obj)
            {
                FarmerProfile f = (FarmerProfile)obj;
                if (cmbMachinery.SelectedIndex > 0)
                {
                    if (!f.Machinery.Contains(cmbMachinery.SelectedItem.ToString()))
                        return false;
                }
                if (cmbLocation.SelectedIndex > 0)
                {
                    if (f.Country != cmbLocation.SelectedItem.ToString())
                        return false;
                }
                if (cmbGrain.SelectedIndex > 0)
                {
                    if (!f.Grain.Contains(cmbGrain.SelectedItem.ToString()))
                        return false;
                }
                if (cmbFacility.SelectedIndex > 0)
                {
                    if (!f.Facility.Contains(cmbFacility.SelectedItem.ToString()))
                        return false;
                }
                if (cmbAnimals.SelectedIndex > 0)
                {
                    if (!f.Animals.Contains(cmbAnimals.SelectedItem.ToString()))
                        return false;
                }
                if (cmbProject.SelectedIndex > 0)
                {
                    if (!f.Project.Contains(cmbProject.SelectedItem.ToString()))
                        return false;
                }
                return true;
            };
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            scrollFilterView.Visibility = Visibility.Collapsed;
            scrollCreateProfile.Visibility = Visibility.Visible;
        }
        private void bbtnCancel_Click(object sender, RoutedEventArgs e)
        {
            scrollFilterView.Visibility = Visibility.Visible;
            scrollCreateProfile.Visibility = Visibility.Collapsed;
        }
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            char[] splitChars = new[] { ',' };
            XElement record = new XElement("Farmer");
            XElement contact = new XElement("Contact_information");
            contact.Add(new XElement("FirstName", txtFName.Text));
            contact.Add(new XElement("LastName", txtLName.Text));
            contact.Add(new XElement("ContactNo", txtPhone.Text));
            contact.Add(new XElement("Email", txtEmail.Text));
            record.Add(contact);
            XElement location = new XElement("Location");
            location.Add(new XElement("City", txtCity.Text));
            location.Add(new XElement("State", txtState.Text));
            location.Add(new XElement("Country", txtCountry.Text));
            record.Add(location);
            XElement size = new XElement("size_of_farm");
            size.Add(new XElement("Total", txtFarmSize.Text));
            string[] subs = txtGrains.Text.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ele in subs)
                size.Add(new XElement(ele));
            record.Add(size);
            XElement animals = new XElement("Animals");
            subs = txtAnimals.Text.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ele in subs)
                animals.Add(new XElement(ele));
            record.Add(animals);
            XElement machinery = new XElement("Machinery");
            subs = txtMachinery.Text.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ele in subs)
                machinery.Add(new XElement("Tractor", ele));
            record.Add(machinery);
            XElement project = new XElement("Project");
            subs = txtProject.Text.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ele in subs)
                project.Add(new XElement(ele));
            record.Add(project);
            XElement facility = new XElement("Facility");
            subs = txtFacility.Text.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ele in subs)
                facility.Add(new XElement(ele.Replace(" ", "")));
            record.Add(facility);

            doc.Root.Add(record);
            visitorProfile = record;
            scrollFilterView.Visibility = Visibility.Visible;
            scrollCreateProfile.Visibility = Visibility.Collapsed;
        }

        private void window_Closed(object sender, EventArgs e)
        {
            doc.Save("data.xml");
        }

        private void btnFarmersLikeMe_Click(object sender, RoutedEventArgs e)
        {
            LoadListBox();
            scrollFilterView.Visibility = Visibility.Visible;
            gridHomePage.Visibility = Visibility.Collapsed;
            listBoxFarmerProfile.Items.Filter = delegate (object obj)
            {
                FarmerProfile f = (FarmerProfile)obj;
                if (cmbMachinery.SelectedIndex > 0)
                {
                    if (!f.Machinery.Contains(cmbMachinery.SelectedItem.ToString()))
                        return false;
                }
                if (cmbLocation.SelectedIndex > 0)
                {
                    if (f.Country != cmbLocation.SelectedItem.ToString())
                        return false;
                }
                if (cmbGrain.SelectedIndex > 0)
                {
                    if (!f.Grain.Contains(cmbGrain.SelectedItem.ToString()))
                        return false;
                }
                if (cmbFacility.SelectedIndex > 0)
                {
                    if (!f.Facility.Contains(cmbFacility.SelectedItem.ToString()))
                        return false;
                }
                if (cmbAnimals.SelectedIndex > 0)
                {
                    if (!f.Animals.Contains(cmbAnimals.SelectedItem.ToString()))
                        return false;
                }
                if (cmbProject.SelectedIndex > 0)
                {
                    if (!f.Project.Contains(cmbProject.SelectedItem.ToString()))
                        return false;
                }
                return true;
            };
        }

        private void btnMachinery_Click(object sender, RoutedEventArgs e)
        {
            LoadListBox();
            scrollFilterView.Visibility = Visibility.Visible;
            gridHomePage.Visibility = Visibility.Collapsed;
            listBoxFarmerProfile.Items.Filter = delegate (object obj)
            {
                FarmerProfile f = (FarmerProfile)obj;
                if (cmbMachinery.SelectedIndex > 0)
                {
                    if (!f.Machinery.Contains(cmbMachinery.SelectedItem.ToString()))
                        return false;
                }
                if (cmbLocation.SelectedIndex > 0)
                {
                    if (f.Country != cmbLocation.SelectedItem.ToString())
                        return false;
                }
                if (cmbGrain.SelectedIndex > 0)
                {
                    if (!f.Grain.Contains(cmbGrain.SelectedItem.ToString()))
                        return false;
                }
                if (cmbFacility.SelectedIndex > 0)
                {
                    if (!f.Facility.Contains(cmbFacility.SelectedItem.ToString()))
                        return false;
                }
                if (cmbAnimals.SelectedIndex > 0)
                {
                    if (!f.Animals.Contains(cmbAnimals.SelectedItem.ToString()))
                        return false;
                }
                if (cmbProject.SelectedIndex > 0)
                {
                    if (!f.Project.Contains(cmbProject.SelectedItem.ToString()))
                        return false;
                }
                try
                {
                    foreach (var ele in visitorProfile.Element("Machinery").Elements())
                    {
                        if (f.Machinery.Contains(ele.Value, StringComparer.InvariantCultureIgnoreCase))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                catch { return false; }
                return true;
            };
        }

        private void btnSize_Click(object sender, RoutedEventArgs e)
        {
            LoadListBox();
            scrollFilterView.Visibility = Visibility.Visible;
            gridHomePage.Visibility = Visibility.Collapsed;
            listBoxFarmerProfile.Items.Filter = delegate (object obj)
            {
                FarmerProfile f = (FarmerProfile)obj;
                
                if (cmbMachinery.SelectedIndex > 0)
                {
                    if (!f.Machinery.Contains(cmbMachinery.SelectedItem.ToString()))
                        return false;
                }
                if (cmbLocation.SelectedIndex > 0)
                {
                    if (f.Country != cmbLocation.SelectedItem.ToString())
                        return false;
                }
                if (cmbGrain.SelectedIndex > 0)
                {
                    if (!f.Grain.Contains(cmbGrain.SelectedItem.ToString()))
                        return false;
                }
                if (cmbFacility.SelectedIndex > 0)
                {
                    if (!f.Facility.Contains(cmbFacility.SelectedItem.ToString()))
                        return false;
                }
                if (cmbAnimals.SelectedIndex > 0)
                {
                    if (!f.Animals.Contains(cmbAnimals.SelectedItem.ToString()))
                        return false;
                }
                if (cmbProject.SelectedIndex > 0)
                {
                    if (!f.Project.Contains(cmbProject.SelectedItem.ToString()))
                        return false;
                }
                int farmSizeVisitor = int.Parse(visitorProfile.Element("size_of_farm").Element("Total").Value);
                if (farmSizeVisitor == f.FarmSize || (f.FarmSize - 100 <= farmSizeVisitor && farmSizeVisitor <= f.FarmSize + 100))
                {
                    return true;

                }
                else
                {
                    return false;
                }

                return true;
            };
        }

        private void btnLocation_Click(object sender, RoutedEventArgs e)
        {
            LoadListBox();
            scrollFilterView.Visibility = Visibility.Visible;
            gridHomePage.Visibility = Visibility.Collapsed;
            listBoxFarmerProfile.Items.Filter = delegate (object obj)
            {
                FarmerProfile f = (FarmerProfile)obj;
                
                if (cmbMachinery.SelectedIndex > 0)
                {
                    if (!f.Machinery.Contains(cmbMachinery.SelectedItem.ToString()))
                        return false;
                }
                if (cmbLocation.SelectedIndex > 0)
                {
                    if (f.Country != cmbLocation.SelectedItem.ToString())
                        return false;
                }
                if (cmbGrain.SelectedIndex > 0)
                {
                    if (!f.Grain.Contains(cmbGrain.SelectedItem.ToString()))
                        return false;
                }
                if (cmbFacility.SelectedIndex > 0)
                {
                    if (!f.Facility.Contains(cmbFacility.SelectedItem.ToString()))
                        return false;
                }
                if (cmbAnimals.SelectedIndex > 0)
                {
                    if (!f.Animals.Contains(cmbAnimals.SelectedItem.ToString()))
                        return false;
                }
                if (cmbProject.SelectedIndex > 0)
                {
                    if (!f.Project.Contains(cmbProject.SelectedItem.ToString()))
                        return false;
                }
                string stateVisitor = visitorProfile.Element("Location").Element("State").Value;
                string countryVisitor = visitorProfile.Element("Location").Element("Country").Value;
                if (f.State == stateVisitor && f.Country == countryVisitor)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                return true;
            };
        }

        private void btnGrain_Click(object sender, RoutedEventArgs e)
        {
            LoadListBox();
            scrollFilterView.Visibility = Visibility.Visible;
            gridHomePage.Visibility = Visibility.Collapsed;
            listBoxFarmerProfile.Items.Filter = delegate (object obj)
            {
                FarmerProfile f = (FarmerProfile)obj;

                if (cmbMachinery.SelectedIndex > 0)
                {
                    if (!f.Machinery.Contains(cmbMachinery.SelectedItem.ToString()))
                        return false;
                }
                if (cmbLocation.SelectedIndex > 0)
                {
                    if (f.Country != cmbLocation.SelectedItem.ToString())
                        return false;
                }
                if (cmbGrain.SelectedIndex > 0)
                {
                    if (!f.Grain.Contains(cmbGrain.SelectedItem.ToString()))
                        return false;
                }
                if (cmbFacility.SelectedIndex > 0)
                {
                    if (!f.Facility.Contains(cmbFacility.SelectedItem.ToString()))
                        return false;
                }
                if (cmbAnimals.SelectedIndex > 0)
                {
                    if (!f.Animals.Contains(cmbAnimals.SelectedItem.ToString()))
                        return false;
                }
                if (cmbProject.SelectedIndex > 0)
                {
                    if (!f.Project.Contains(cmbProject.SelectedItem.ToString()))
                        return false;
                }

                try
                {
                    foreach (var ele in visitorProfile.Element("size_of_farm").Elements().Where(d => d.Name.LocalName != "Total"))
                    {
                        if (f.Grain.Contains(ele.Name.LocalName, StringComparer.InvariantCultureIgnoreCase))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                catch { return false; }
                return true;
            };
        }

        private void btnNews_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Coming soon.","Message");
        }
        private void btnTechnology_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Coming soon.", "Message");
        }
        private void btnMaintenance_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Coming soon.", "Message");
        }

        private void btnCropProtection_Click(object sender, RoutedEventArgs e)
        {
            LoadListBox();
            scrollFilterView.Visibility = Visibility.Visible;
            gridHomePage.Visibility = Visibility.Collapsed;
            listBoxFarmerProfile.Items.Filter = delegate (object obj)
            {
                FarmerProfile f = (FarmerProfile)obj;
                if (cmbMachinery.SelectedIndex > 0)
                {
                    if (!f.Machinery.Contains(cmbMachinery.SelectedItem.ToString()))
                        return false;
                }
                if (cmbLocation.SelectedIndex > 0)
                {
                    if (f.Country != cmbLocation.SelectedItem.ToString())
                        return false;
                }
                if (cmbGrain.SelectedIndex > 0)
                {
                    if (!f.Grain.Contains(cmbGrain.SelectedItem.ToString()))
                        return false;
                }
                if (cmbFacility.SelectedIndex > 0)
                {
                    if (!f.Facility.Contains(cmbFacility.SelectedItem.ToString()))
                        return false;
                }
                if (cmbAnimals.SelectedIndex > 0)
                {
                    if (!f.Animals.Contains(cmbAnimals.SelectedItem.ToString()))
                        return false;
                }
                if (cmbProject.SelectedIndex > 0)
                {
                    if (!f.Project.Contains(cmbProject.SelectedItem.ToString()))
                        return false;
                }
                return true;
            };
        }
    }
}
