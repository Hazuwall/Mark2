using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Plugins.Motion.Contracts
{
    public interface IMotionServiceContract
    {
        [Display(Description = "Получить обобщённые координаты рабочего органа")]
        Vector6 GetCoords();

        Vector6 GetAbsCoords();

        Vector6 GetVelocities();

        Vector6 GetAbsVelocities();

        [Role(Role.Admin)]
        void Freeze();

        [DisplayName("Изменены координаты рабочего органа")]
        event EventHandler CoordsChanged;
    }
}
