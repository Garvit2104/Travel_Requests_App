namespace Travel_Requests_App.DTOs.Grades
{
    public class UserRequestDTO
    {
        public int employee_id { get; set; }

        public string? first_name { get; set; }

        public string? last_name { get; set; }
        public string? phone_number { get; set; }

        public string? email_address { get; set; }

        public string? role { get; set; }

        public int? current_grade_id { get; set; }
    }
}
