using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Position {
    private int r;
    private int c;


    public Position(int r = 0 , int c = 0) {
        this.r = r;
        this.c = c;
    }


    public int R {
        get { return r; }
        set { r = value; }
    }
    public int C {
        get { return c; }
        set { c = value; }
    }


    public MatrixState GetNorthState() {

        MatrixState state;

        GameMgr.Instance.GetMatrixState(new Position(r - 1 , c) , out state);

        return state;
    }
    public MatrixState GetEastState() {

        MatrixState state;

        GameMgr.Instance.GetMatrixState(new Position(r , c+1) , out state);

        return state;
    }
    public MatrixState GetSouthState() {

        MatrixState state;

        GameMgr.Instance.GetMatrixState(new Position(r + 1 , c) , out state);

        return state;
    }
    public MatrixState GetWestState() {

        MatrixState state;

        GameMgr.Instance.GetMatrixState(new Position(r   , c-1) , out state);

        return state;
    }


    public int GetDistance(Position destPos , out int rRemain, out int cRemain) {
        rRemain = Mathf.Abs(r - destPos.R);
        cRemain = Mathf.Abs(c - destPos.C);


        return rRemain + cRemain;
    }
}
