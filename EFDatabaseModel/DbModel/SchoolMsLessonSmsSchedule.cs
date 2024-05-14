using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsLessonSmsSchedule
    {
        public long Id { get; set; }
        public long SchoolMsLessonId { get; set; }
        public long SchoolMsStudentId { get; set; }
        public long SchoolMsSmsToSendId { get; set; }

        public virtual SchoolMsLesson SchoolMsLesson { get; set; }
        public virtual SchoolMsSmsToSend SchoolMsSmsToSend { get; set; }
        public virtual SchoolMsStudent SchoolMsStudent { get; set; }
    }
}
