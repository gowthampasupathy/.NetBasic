dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Tools

dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL


     dotnet ef migrations add init

    
     dotnet ef database update 


Could not execute because the specified command or file was not found.
Possible reasons for this include:
  * You misspelled a built-in dotnet command.
  * You intended to execute a .NET program, but dotnet-ef does not exist.
  * You intended to run a global tool, but a dotnet-prefixed executable with this name could not be found on the PATH.


dotnet tool install --global dotnet-ef


http://localhost:5226/api/employees



[{"id":"71bae843-972c-4b63-b9b7-48eeded2cb99","name":"John Doe","email":"johndoe@example.com"}]

INSERT INTO public."Employees" ("Id", "Name", "Email")
VALUES (gen_random_uuid(), 'John Doe', 'johndoe@example.com');







package com.rent.rent;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.*;


@RestController
@RequestMapping("/")
@CrossOrigin("http://localhost:3000/")
public class Controllerclass {
    
    private final Serviceclass serviceclass;
    @Autowired
    public Controllerclass(Serviceclass serviceclass){
        this.serviceclass=serviceclass;
    }

    @GetMapping("/cardetails")
    public List<Car> getallcar() {
        List<Car> cars= serviceclass.getcardetail();
        return cars;
    }
    @PostMapping("/addcar")
    public String addcar(@RequestBody Car newCar){
        serviceclass.addcars(newCar);
        return "redirect:/cardetails";
    }
    
}


service



package com.rent.rent;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;



@Service
public class Serviceclass {
    private final Carrepo carrepo;

    @Autowired
    public Serviceclass(Carrepo carrepo){
        this.carrepo=carrepo;
    }

    public List<Car> getcardetail(){
        return carrepo.getcardetails();
    }
    public void addcars(Car car){
        carrepo.addcar(car);
    }

}









package com.rent.rent;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.simple.JdbcClient;
import org.springframework.jdbc.support.GeneratedKeyHolder;
import org.springframework.jdbc.support.KeyHolder;
import org.springframework.stereotype.Repository;

@Repository
public class Carrepo {

    private final JdbcClient jdbcClient ;
    Carrepo(JdbcTemplate template,JdbcClient jdbcClient){
        this.jdbcClient=jdbcClient;
    }

    public java.util.List<Car> getcardetails(){
         return jdbcClient.sql("SELECT * FROM CAR").query(
           (req,res)->{
            return new Car(req.getLong("id"),req.getString("imgurl"),req.getString("car_name"),req.getString("gear_type"),req.getInt("seat_no"),req.getInt("price"));
           }
        ).list();
    }
    public void addcar(Car car){
        KeyHolder keyHolder =new GeneratedKeyHolder();
        jdbcClient.sql("INSERT INTO CAR (IMGURL,CAR_NAME,GEAR_TYPE,SEAT_NO,PRICE) VALUES (?,?,?,?,?)")
        .params(car.getImgurl(),car.getCar_name(),car.getGear_type(),car.getSeat_no(),car.getPrice()).update(keyHolder);
    }
    
    
}


const mongoose =require('mongoose')
const userschema=new mongoose.Schema({
    name:String,
    email:String,
    password:String,
    role:{
        type:String,
        default:"user"
    }

})
const usermodal=mongoose.model("register",userschema)
module.exports=usermodal;







