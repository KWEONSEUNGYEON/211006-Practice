﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myCarManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            try
            {
                //DataManager 자체에 접근하는 순간! DataManager 안의
                //static DataManager 생성자(정적 생성자)가 호출됨
                textBox_parkingSpot.Text = DataManager.Cars[0].ParkingSpot.ToString();
                textBox_carNumber.Text = DataManager.Cars[0].CarNumber.ToString();
                textBox_driverName.Text = DataManager.Cars[0].DriverName.ToString();
                textBox_phoneNumber.Text = DataManager.Cars[0].PhoneNumber.ToString();
            }   
            catch (Exception ex)
            {
                WriteLog("초창기 데이터 없음. 주차 공간을 하나 이상 생성하세요.");
            }
            if(DataManager.Cars.Count > 0)
                dataGridView_parkingManager.DataSource = DataManager.Cars;

            button_add.Click += delegate (object sender, EventArgs e)
            {
                if(int.TryParse(textBox_parkingSpot_lookUp.Text, out int parkingSpot) == false)
                {
                    MessageBox.Show("주차공간번호는 숫자여야 합니다.");
                    return;
                }
                if(parkingSpot <= 0)
                {
                    MessageBox.Show("주차공간번호는 0 이상의 값이어야 합니다.");
                    return;
                }

                string contents = string.Empty;
                if(DataManager.Save("insert", parkingSpot, out contents))
                {
                    button_refresh.PerformClick();
                }
                WriteLog(contents);
            };

            button_delete.Click += (sender, e) =>
            {
                if (int.TryParse(textBox_parkingSpot_lookUp.Text, out int parkingSpot) == false)
                {
                    MessageBox.Show("주차공간번호는 숫자여야 합니다.");
                    return;
                }
                if (parkingSpot <= 0)
                {
                    MessageBox.Show("주차공간번호는 0 이상의 값이어야 합니다.");
                    return;
                }

                string contents = string.Empty;
                if (DataManager.Save("delete", parkingSpot, out contents))
                {
                    button_refresh.PerformClick();
                }
                WriteLog(contents);
            };
        }

        private void timer_now_Tick(object sender, EventArgs e)
        {
            label_now.Text =
                $"현재시간 : {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}";
        }


        private bool checkParkingSpot(ParkingCar parkingCar)
        {
            return parkingCar.ParkingSpot.ToString() == textBox_parkingSpot.Text;
        }
        //주차 
        private void button_parkingAdd_Click(object sender, EventArgs e)
        {
            //Trim 양 옆의 공백제거
            //        이 동 준       // 이런 게 있다면
            //이 동 준//이런식으로 바꿈
            if (textBox_parkingSpot.Text.Trim() == "")
                MessageBox.Show("주차 공간을 입력하세요.");
            else if (textBox_carNumber.Text.Trim() == "")
                MessageBox.Show("차량 번호를 입력해주세요.");
            else
            {
                try
                {
                    //내가 주차하려는 공간에 차가 있는지, 없는지 체크
                    //ParkingCar car = DataManager.Cars.Single
                    //    ((x)=>x.ParkingSpot.ToString()==textBox_parkingSpot.Text);

                    //  ParkingCar car = DataManager.Cars.Single
                    //      (delegate (ParkingCar parkingCar)
                    //      { return parkingCar.ParkingSpot.ToString() == textBox_parkingSpot.Text; });

                    ParkingCar car = DataManager.Cars.Single(checkParkingSpot);
                    if (car.CarNumber.Trim() != "")
                        MessageBox.Show("이미 해당 공간에 차가 있음");
                    else //차가 없는 경우
                    {
                        //car의 멤버변수에 값을 넣으면
                        //car는 Cars랑 연결되어 있으므로, Cars에 있는 해당 주차공간 자료가 변경됨
                        car.CarNumber = textBox_carNumber.Text;
                        car.DriverName = textBox_driverName.Text;
                        car.PhoneNumber = textBox_phoneNumber.Text;
                        car.ParkingTime = DateTime.Now;

                        dataGridView_parkingManager.DataSource = null;
                        dataGridView_parkingManager.DataSource = DataManager.Cars;

                        DataManager.Save(car.ParkingSpot, car.CarNumber, car.DriverName, car.PhoneNumber);
                        string contents = $"주차공간 {textBox_parkingSpot.Text}에 {textBox_carNumber.Text}차를 주차했습니다.";
                        WriteLog(contents);

                    }
                }
                catch (Exception ex)
                {
                    string contents = $"주차공간 {textBox_parkingSpot.Text}은(는) 없습니다.";
                    WriteLog(contents); //listBox에 내용도 적고 메시지 박스도 띄워주는 것
                    //MessageBox.Show(contents);

                }
            }
        }

        private void WriteLog(string contents)
        {
            string logContents = $"[{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")}]{contents}";
            DataManager.PrintLog(logContents);
            MessageBox.Show(contents);
            listBox_logPrint.Items.Insert(0, logContents); //최신 내용이 위로 올라가고 기존 내용은 밑으로 내려감
            //Insert 대신 Add(logContents)를 하면 최신 내용이 밑으로 내려감.
        }

        //출차
        private void button_parkingRemove_Click(object sender, EventArgs e)
        {
            if (textBox_parkingSpot.Text.Trim() == "")
                MessageBox.Show("주차공간을 입력하세요.");
            else
            {
                try
                {
                    ParkingCar car = DataManager.Cars.Single((x) => x.ParkingSpot.ToString() == textBox_parkingSpot.Text);
                    string oldCar = car.CarNumber; //기존에 주차된 차넘버
                    car.CarNumber = "";
                    car.DriverName = "";
                    car.PhoneNumber = "";
                    car.ParkingTime = new DateTime();

                    dataGridView_parkingManager.DataSource = null;
                    dataGridView_parkingManager.DataSource = DataManager.Cars;

                    //DB에 정보 보냄(테이블 변경)
                    //해당 공간에 정보를 다 지우는 메소드
                    //DataManager.Save(int.Parse(textBox_parkingSpot.Text), "", "", "", true);
                    DataManager.Save(car.ParkingSpot, "", "", "", true);
                    string contents = $"주차공간 {textBox_parkingSpot.Text}에서 {oldCar}차가 출차했습니다.";
                    WriteLog(contents);
                }
                catch (Exception ex) //주차공간이 없는 경우
                {
                    string contents = $"주차공간 {textBox_parkingSpot.Text}는 없습니다.";
                    WriteLog(contents);
                }

            }
        }

        private void dataGridView_parkingManager_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ParkingCar car = dataGridView_parkingManager.CurrentRow.DataBoundItem as ParkingCar;
                textBox_parkingSpot.Text = car.ParkingSpot.ToString();
                textBox_carNumber.Text = car.CarNumber;
                textBox_driverName.Text = car.DriverName;
                textBox_phoneNumber.Text = car.PhoneNumber;
            }
            catch (Exception ex)
            {
            }
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            DataManager.Load();
            dataGridView_parkingManager.DataSource = null;
            if(DataManager.Cars.Count> 0)
                dataGridView_parkingManager.DataSource = DataManager.Cars;
        }

        private void button_selected_lookUp_Click(object sender, EventArgs e)
        {
            try
            {
                int parkingSpot = int.Parse(textBox_parkingSpot_lookUp.Text);
                string ParkingCar = lookUpParkingSpot(parkingSpot);
                string contents;
                if (ParkingCar == "해당주차공간없음")
                {
                    contents = $"해당 주차 공간은 존재하지 않습니다. ({parkingSpot})";
                }
                else if(ParkingCar != "")
                {
                    contents = $"주차 공간 {parkingSpot}에 주차되어 있는 차는 {ParkingCar}입니다.";
                }
                else
                {
                    contents = $"주차 공간 {parkingSpot}에 주차되어 있는 차가 없습니다.";
                }
                WriteLog(contents);
            }
            catch (Exception ex)
            {
                WriteLog($"{textBox_parkingSpot_lookUp.Text} 값은 잘못되었습니다.");
            }
        }

        private string lookUpParkingSpot(int parkingSpot)
        {
            foreach (var item in DataManager.Cars)
            {
                if(item.ParkingSpot == parkingSpot)
                    return item.CarNumber.ToString();
            }
            return "해당주차공간없음";
        }

        private void button_parkingSpot_check_Click(object sender, EventArgs e)
        {
            SpotForm sf = new SpotForm(this);
            sf.Show();
        }
    }
}
