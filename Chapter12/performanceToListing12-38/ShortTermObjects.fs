module ShortTermObjects

// Code as Listing 12-21
type Point3d(x : float, y : float, z : float) =
    member __.X = x
    member __.Y = y
    member __.Z = z
    member val Description = "" with get, set
    member this.DistanceFrom(that : Point3d) =
        (that.X - this.X) ** 2. +
        (that.Y - this.Y) ** 2. +
        (that.Z - this.Z) ** 2.
        |> sqrt
    override this.ToString() =
        sprintf "X: %f, Y: %f, Z: %f" this.X this.Y this.Z

type Float3 = (float * float * float)

module Old =
    let withinRadius (radius : float) (here : Float3) (coords : Float3[]) =
        let here = Point3d(here)
        coords
        |> Array.map Point3d
        |> Array.filter (fun there ->
            there.DistanceFrom(here) <= radius)
        |> Array.map (fun p3d -> p3d.X, p3d.Y, p3d.Z)
        
module New =
    let withinRadius (radius : float) (here : Float3) (coords : Float3[]) =
        let distance x1 y1 z1 x2 y2 z2 =
            let dx = x1 - x2
            let dy = y1 - y2
            let dz = z1 - z2
            dx * dx +
            dy * dy +
            dz * dz
            |> sqrt
        let x1, y1, z1 = here
        coords
        |> Array.filter (fun (x2, y2, z2) ->
            distance x1 y1 z1 x2 y2 z2 <= radius)