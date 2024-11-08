using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GuardianAngels.Models;
using Microsoft.AspNet.Identity;
using QRCoder;

namespace GuardianAngels.Controllers
{
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Students
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.School);          
            return View(students.ToList());
        }

        public ActionResult DbStudents()
        {
            var students = db.Students.Include(s => s.School);
            return View(students.ToList());
        }

        public ActionResult schools()
        {
            return View(db.Schools.ToList());
        }

        public ActionResult selectschools()
        {
            return View(db.Schools.ToList());
        }

        public ActionResult AdminIndex(int id)
        {
            var students = db.Students.Where(a=>a.SchoolId==id).Include(s => s.School);
            return View(students.ToList());
        }



        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }


            //AKA-BryanstonDrive
            var vName = db.Vehicles.Where(a=>a.VehicleId == student.vId).Select(a => a.Name).FirstOrDefault();
            var vPlate = db.Vehicles.Where(a => a.VehicleId == student.vId).Select(a => a.Plate).FirstOrDefault();
            var color = db.Vehicles.Where(a => a.VehicleId == student.vId).Select(a => a.Color).FirstOrDefault();

            ViewBag.VName = vName;
            ViewBag.VPlate = vPlate;
            ViewBag.Color = color; 

            return View(student);
        }

        // GET: Students/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.SchoolId = new SelectList(db.Schools, "SchoolId", "SchoolName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,Name,Surname,Age,StudentImage,SchoolId,ContactNumber,ParentEmail,QR,Status,vId")] Student student)
        {
            if (ModelState.IsValid)
            {
                //cici-iqiniso
                HttpPostedFileBase file = Request.Files["ImageFile"];
                //Simmy
                if (file != null && file.ContentLength > 0)
                {
                    // Read the file content into a byte array
                    byte[] fileBytes;
                    using (BinaryReader reader = new BinaryReader(file.InputStream))
                    {
                        fileBytes = reader.ReadBytes(file.ContentLength);
                    }

                    // Convert the byte array to a Base64 string
                    string base64String = Convert.ToBase64String(fileBytes);

                    // Set the ImageData property to the Base64 string
                   student.StudentImage = base64String;
                }

                var schoolVID = db.Vehicles.Where(a=> a.SchoolId == student.SchoolId).Select(a=>a.VehicleId).FirstOrDefault();

                student.QR = student.StudentId.ToString();
                student.Status = "Home";
                student.ParentEmail = User.Identity.GetUserName();
                student.vId = schoolVID;

                db.Students.Add(student);
                db.SaveChanges();
                SendEmailWithAttachment(student.ParentEmail,student.Name,student.StudentId.ToString());
                return RedirectToAction("Details", new {id = student.StudentId });
            }

            ViewBag.SchoolId = new SelectList(db.Schools, "SchoolId", "SchoolName", student.SchoolId);
            return View(student);
        }
       
        public void SendEmailWithAttachment(string email, string studentName, string id)
        {
            // SMTP server configuration
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("guardianangels02024@gmail.com", "cdmy fhrf arjo lqbw"),
                EnableSsl = true,
            };

            // Email message setup
            var mailMessage = new MailMessage
            {
                From = new MailAddress("guardianangels02024@gmail.com"),
                Subject = "Registered Student[QR CODE BELOW]",
                Body = "You Have Successfully Registered " + studentName + ". Please Download & Print the QRCODE & Ensure That Sudent Carries the QR CODE At ALL Times",
                IsBodyHtml = true, // Set to true if you want to send HTML email
            };
            mailMessage.To.Add(email);

            // Generate QR code based on the student's name
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(id, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                    {
                        // Convert QR code image to a memory stream
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            qrCodeImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                            memoryStream.Position = 0; // Reset the stream position

                            // Create the attachment from the memory stream
                            Attachment qrAttachment = new Attachment(memoryStream, "QRCode.png", MediaTypeNames.Image.Jpeg);
                            mailMessage.Attachments.Add(qrAttachment);

                            // Send the email
                            try
                            {
                                smtpClient.Send(mailMessage);
                                Console.WriteLine("Email with QR code sent successfully.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error sending email: {ex.Message}");
                            }
                        }
                    }
                }
            }
        }

            // GET: Students/Edit/5
            public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchoolId = new SelectList(db.Schools, "SchoolId", "SchoolName", student.SchoolId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,Name,Surname,Age,StudentImage,SchoolId,ContactNumber,ParentEmail,QR,Status,vId")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SchoolId = new SelectList(db.Schools, "SchoolId", "SchoolName", student.SchoolId);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DriverUpdateStudents(int id)
        {
            var students = db.Students.Where(a=>a.vId == id).Include(s => s.School);
            return View(students.ToList());
        }

        public ActionResult SchoolUpdateStudents()
        {
            var students = db.Students.Include(s => s.School);
            return View(students.ToList());
        }

        [HttpPost]
        public ActionResult UpdateStudentStatus(int studentId, string status)
        {
            var student = db.Students.Find(studentId);
            string pnum = db.Users.Where(d => d.Email == student.ParentEmail).Select(a => a.number).FirstOrDefault();
            if (student != null)
            {
                student.Status = status;

                if (student.Status == "Arrived At School")
                {
                    Schoolregister schoolregister = new Schoolregister();

                    schoolregister.studentid = studentId;
                    schoolregister.studentName = student.Name + " " + student.Surname;
                    schoolregister.Status  = status; 
                    schoolregister.Date = DateTime.Now;
                    schoolregister.studentImage = student.StudentImage;

                    db.schoolregisters.Add(schoolregister);
                }

                db.SaveChanges();

                SendSms(pnum, $"Location Of Student Has Changed:\n {student.Name} { student.Surname} :Now {student.Status}.");
                return Json(new { success = true });
            }

            //AddToRegister
            

            return Json(new { success = false, message = "Student not found" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult GetAttendanceDates()
        {
            // Retrieve all attendance dates and format them as yyyy-MM-dd
            var attendanceDates = db.schoolregisters
                                    .GroupBy(r => DbFunctions.TruncateTime(r.Date))  // Group by date only
                                    .Select(g => g.Key.ToString())                   // Convert to string
                                    .ToList();

            // Pass dates to ViewBag to use in the calendar
            ViewBag.AttendanceDates = attendanceDates;

            return View();
        }

        public ActionResult RecordsByDate(DateTime date)
        {
            // Retrieve attendance records for the specific date
            var attendanceRecords = db.schoolregisters
                                      .Where(r => DbFunctions.TruncateTime(r.Date) == date.Date)
                                      .ToList();

            // Pass the selected date to ViewBag
            ViewBag.SelectedDate = date.ToString("yyyy-MMMM-dd");

            return View(attendanceRecords);
        }


        private void SendSms(string phoneNumber, string message)
        {
            string myURI = "https://api.bulksms.com/v1/messages";
            string myUsername = "guardianangels2024";
            string myPassword = "Bhebhe@2002";
            string myData = $"{{to: \"{phoneNumber}\", body:\"{message}\"}}";

            var request = WebRequest.Create(myURI);
            request.Credentials = new NetworkCredential(myUsername, myPassword);
            request.PreAuthenticate = true;
            request.Method = "POST";
            request.ContentType = "application/json";

            var encoding = new UnicodeEncoding();
            var encodedData = encoding.GetBytes(myData);

            var stream = request.GetRequestStream();
            stream.Write(encodedData, 0, encodedData.Length);
            stream.Close();

            try
            {
                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                Console.WriteLine(reader.ReadToEnd());
            }
            catch (WebException ex)
            {
                Console.WriteLine("An error occurred:" + ex.Message);
                var reader = new StreamReader(ex.Response.GetResponseStream());
                Console.WriteLine("Error details:" + reader.ReadToEnd());
            }
        }

    }
}
