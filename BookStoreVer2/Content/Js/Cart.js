.cart - wrapper {
    display: block;
    height: 100px;
    width: 100px;
    margin: 0 auto;
    position: relative;
    top: 150px;
}

.cart - body {
    display: block;
    height: 100px;
    width: 100px;
    background - color: grey;
    margin: 0 auto;
    position: relative;
    top: 30px;
    right: 20px;
  
  &:hover {
        background - color: red;
    }
}

.cart - handle {
    display: block;
    height: 60px;
    width: 60px;
    background - color: #fff;
    border: 6px solid grey;
    position: relative;
    bottom: 0px;
    border - radius: 60px;
    text - align: center;
  
  &:hover {
        border - color: red;
    }
}

.cart - hole1,
.cart - hole2 {
    top: 10px;
    width: 6px;
    height: 6px;
    background - color: #fff;
    border - radius: 30px;
}

.cart - hole1 {
    position: absolute;
    left: 14px;
}

.cart - hole2 {
    position: absolute;
    right: 14px;
}

.cart - items {
    color: #fff;
    position: relative;
    top: 28px;
    font - family: Helvetica;
    font - weight: 600;
    font - size: 30px;
}