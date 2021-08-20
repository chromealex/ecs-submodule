
namespace ME.ECS.Filters {

    public delegate void FD(in Entity e);
    
    public delegate void FDW<T0>(in Entity e, ref T0 t0);
public delegate void FDR<T0>(in Entity e, in T0 t0);
public delegate void FDWW<T0,T1>(in Entity e, ref T0 t0,ref T1 t1);
public delegate void FDRW<T0,T1>(in Entity e, in T0 t0,ref T1 t1);
public delegate void FDRR<T0,T1>(in Entity e, in T0 t0,in T1 t1);
public delegate void FDWWW<T0,T1,T2>(in Entity e, ref T0 t0,ref T1 t1,ref T2 t2);
public delegate void FDRWW<T0,T1,T2>(in Entity e, in T0 t0,ref T1 t1,ref T2 t2);
public delegate void FDRRW<T0,T1,T2>(in Entity e, in T0 t0,in T1 t1,ref T2 t2);
public delegate void FDRRR<T0,T1,T2>(in Entity e, in T0 t0,in T1 t1,in T2 t2);
public delegate void FDWWWW<T0,T1,T2,T3>(in Entity e, ref T0 t0,ref T1 t1,ref T2 t2,ref T3 t3);
public delegate void FDRWWW<T0,T1,T2,T3>(in Entity e, in T0 t0,ref T1 t1,ref T2 t2,ref T3 t3);
public delegate void FDRRWW<T0,T1,T2,T3>(in Entity e, in T0 t0,in T1 t1,ref T2 t2,ref T3 t3);
public delegate void FDRRRW<T0,T1,T2,T3>(in Entity e, in T0 t0,in T1 t1,in T2 t2,ref T3 t3);
public delegate void FDRRRR<T0,T1,T2,T3>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3);
public delegate void FDWWWWW<T0,T1,T2,T3,T4>(in Entity e, ref T0 t0,ref T1 t1,ref T2 t2,ref T3 t3,ref T4 t4);
public delegate void FDRWWWW<T0,T1,T2,T3,T4>(in Entity e, in T0 t0,ref T1 t1,ref T2 t2,ref T3 t3,ref T4 t4);
public delegate void FDRRWWW<T0,T1,T2,T3,T4>(in Entity e, in T0 t0,in T1 t1,ref T2 t2,ref T3 t3,ref T4 t4);
public delegate void FDRRRWW<T0,T1,T2,T3,T4>(in Entity e, in T0 t0,in T1 t1,in T2 t2,ref T3 t3,ref T4 t4);
public delegate void FDRRRRW<T0,T1,T2,T3,T4>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,ref T4 t4);
public delegate void FDRRRRR<T0,T1,T2,T3,T4>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,in T4 t4);
public delegate void FDWWWWWW<T0,T1,T2,T3,T4,T5>(in Entity e, ref T0 t0,ref T1 t1,ref T2 t2,ref T3 t3,ref T4 t4,ref T5 t5);
public delegate void FDRWWWWW<T0,T1,T2,T3,T4,T5>(in Entity e, in T0 t0,ref T1 t1,ref T2 t2,ref T3 t3,ref T4 t4,ref T5 t5);
public delegate void FDRRWWWW<T0,T1,T2,T3,T4,T5>(in Entity e, in T0 t0,in T1 t1,ref T2 t2,ref T3 t3,ref T4 t4,ref T5 t5);
public delegate void FDRRRWWW<T0,T1,T2,T3,T4,T5>(in Entity e, in T0 t0,in T1 t1,in T2 t2,ref T3 t3,ref T4 t4,ref T5 t5);
public delegate void FDRRRRWW<T0,T1,T2,T3,T4,T5>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,ref T4 t4,ref T5 t5);
public delegate void FDRRRRRW<T0,T1,T2,T3,T4,T5>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,in T4 t4,ref T5 t5);
public delegate void FDRRRRRR<T0,T1,T2,T3,T4,T5>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,in T4 t4,in T5 t5);
public delegate void FDWWWWWWW<T0,T1,T2,T3,T4,T5,T6>(in Entity e, ref T0 t0,ref T1 t1,ref T2 t2,ref T3 t3,ref T4 t4,ref T5 t5,ref T6 t6);
public delegate void FDRWWWWWW<T0,T1,T2,T3,T4,T5,T6>(in Entity e, in T0 t0,ref T1 t1,ref T2 t2,ref T3 t3,ref T4 t4,ref T5 t5,ref T6 t6);
public delegate void FDRRWWWWW<T0,T1,T2,T3,T4,T5,T6>(in Entity e, in T0 t0,in T1 t1,ref T2 t2,ref T3 t3,ref T4 t4,ref T5 t5,ref T6 t6);
public delegate void FDRRRWWWW<T0,T1,T2,T3,T4,T5,T6>(in Entity e, in T0 t0,in T1 t1,in T2 t2,ref T3 t3,ref T4 t4,ref T5 t5,ref T6 t6);
public delegate void FDRRRRWWW<T0,T1,T2,T3,T4,T5,T6>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,ref T4 t4,ref T5 t5,ref T6 t6);
public delegate void FDRRRRRWW<T0,T1,T2,T3,T4,T5,T6>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,in T4 t4,ref T5 t5,ref T6 t6);
public delegate void FDRRRRRRW<T0,T1,T2,T3,T4,T5,T6>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,in T4 t4,in T5 t5,ref T6 t6);
public delegate void FDRRRRRRR<T0,T1,T2,T3,T4,T5,T6>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,in T4 t4,in T5 t5,in T6 t6);
public delegate void FDWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>(in Entity e, ref T0 t0,ref T1 t1,ref T2 t2,ref T3 t3,ref T4 t4,ref T5 t5,ref T6 t6,ref T7 t7);
public delegate void FDRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>(in Entity e, in T0 t0,ref T1 t1,ref T2 t2,ref T3 t3,ref T4 t4,ref T5 t5,ref T6 t6,ref T7 t7);
public delegate void FDRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>(in Entity e, in T0 t0,in T1 t1,ref T2 t2,ref T3 t3,ref T4 t4,ref T5 t5,ref T6 t6,ref T7 t7);
public delegate void FDRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>(in Entity e, in T0 t0,in T1 t1,in T2 t2,ref T3 t3,ref T4 t4,ref T5 t5,ref T6 t6,ref T7 t7);
public delegate void FDRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,ref T4 t4,ref T5 t5,ref T6 t6,ref T7 t7);
public delegate void FDRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,in T4 t4,ref T5 t5,ref T6 t6,ref T7 t7);
public delegate void FDRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,in T4 t4,in T5 t5,ref T6 t6,ref T7 t7);
public delegate void FDRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,in T4 t4,in T5 t5,in T6 t6,ref T7 t7);
public delegate void FDRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,in T4 t4,in T5 t5,in T6 t6,in T7 t7);
public delegate void FDWWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>(in Entity e, ref T0 t0,ref T1 t1,ref T2 t2,ref T3 t3,ref T4 t4,ref T5 t5,ref T6 t6,ref T7 t7,ref T8 t8);
public delegate void FDRWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>(in Entity e, in T0 t0,ref T1 t1,ref T2 t2,ref T3 t3,ref T4 t4,ref T5 t5,ref T6 t6,ref T7 t7,ref T8 t8);
public delegate void FDRRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>(in Entity e, in T0 t0,in T1 t1,ref T2 t2,ref T3 t3,ref T4 t4,ref T5 t5,ref T6 t6,ref T7 t7,ref T8 t8);
public delegate void FDRRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>(in Entity e, in T0 t0,in T1 t1,in T2 t2,ref T3 t3,ref T4 t4,ref T5 t5,ref T6 t6,ref T7 t7,ref T8 t8);
public delegate void FDRRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,ref T4 t4,ref T5 t5,ref T6 t6,ref T7 t7,ref T8 t8);
public delegate void FDRRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,in T4 t4,ref T5 t5,ref T6 t6,ref T7 t7,ref T8 t8);
public delegate void FDRRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,in T4 t4,in T5 t5,ref T6 t6,ref T7 t7,ref T8 t8);
public delegate void FDRRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,in T4 t4,in T5 t5,in T6 t6,ref T7 t7,ref T8 t8);
public delegate void FDRRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7,T8>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,in T4 t4,in T5 t5,in T6 t6,in T7 t7,ref T8 t8);
public delegate void FDRRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7,T8>(in Entity e, in T0 t0,in T1 t1,in T2 t2,in T3 t3,in T4 t4,in T5 t5,in T6 t6,in T7 t7,in T8 t8);


}