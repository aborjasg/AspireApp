﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireApp.ServiceDefaults.Models
{
    public interface IPictureEngine
    {
        void DrawPictureTitle();
        string MakePicture();
    }
}
