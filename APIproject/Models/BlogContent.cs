using System;
using System.Collections.Generic;

namespace APIproject.Models;

public partial class BlogContent
{
    public short Bid { get; set; }

    public string SectionHeading { get; set; }

    public string SectionImage { get; set; }

    public string SectionDescription { get; set; }

    public DateOnly Postdate { get; set; }

    public short? UploadUserId { get; set; }

    public virtual UserList UploadUser { get; set; }
}
