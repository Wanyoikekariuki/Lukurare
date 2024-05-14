using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsClassSubjectTopic
    {
        public SchoolMsClassSubjectTopic()
        {
            SchoolMsLessonGuides = new HashSet<SchoolMsLessonGuide>();
        }

        public long Id { get; set; }
        public long? SchoolMsClassId { get; set; }
        public long SchoolMsSubjectId { get; set; }
        public string TopicName { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual SchoolMsClass SchoolMsClass { get; set; }
        public virtual SchoolMsSubject SchoolMsSubject { get; set; }
        public virtual ICollection<SchoolMsLessonGuide> SchoolMsLessonGuides { get; set; }
    }
}
