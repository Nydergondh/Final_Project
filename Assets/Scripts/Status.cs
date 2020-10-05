using System.Collections;
using System.Collections.Generic;

public class Status
{
    private bool isInvisible;
    private bool timeSlowed;

    #region get-setter
    public bool GetInvisible() {
        return isInvisible;
    }

    public void SetInvisible(bool value) {
        isInvisible = value;
    }

    public bool GetTimeSlowed() {
        return timeSlowed;
    }

    public void SetTimeSlowed(bool value) {
        timeSlowed = value;
    }

    //public void GetInvisible() {

    //}

    //public void SetInvisible() {

    //}

    //public void GetInvisible() {

    //}
    #endregion
}
