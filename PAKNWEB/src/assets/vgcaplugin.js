var _0x071d = ['\x77\x73\x73\x3a\x2f\x2f\x31\x32\x37\x2e\x30\x2e\x30\x2e\x31\x3a\x38\x39\x38\x37\x2f\x53\x69\x67\x6e\x50\x44\x46', '\x77\x73\x73\x3a\x2f\x2f\x31\x32\x37\x2e\x30\x2e\x30\x2e\x31\x3a\x38\x39\x38\x37\x2f\x56\x65\x72\x69\x66\x79\x50\x44\x46', '\x77\x73\x73\x3a\x2f\x2f\x31\x32\x37\x2e\x30\x2e\x30\x2e\x31\x3a\x38\x39\x38\x37\x2f\x53\x69\x67\x6e\x46\x69\x6c\x65', '\x77\x73\x73\x3a\x2f\x2f\x31\x32\x37\x2e\x30\x2e\x30\x2e\x31\x3a\x38\x39\x38\x37\x2f\x47\x65\x74\x4c\x69\x63\x65\x6e\x73\x65\x52\x65\x71\x75\x65\x73\x74', '\x72\x65\x71\x75\x65\x73\x74', '\x57\x65\x62\x53\x6f\x63\x6b\x65\x74', '\x77\x73\x73\x3a\x2f\x2f\x31\x32\x37\x2e\x30\x2e\x30\x2e\x31\x3a\x38\x39\x38\x37\x2f\x43\x6f\x6e\x66\x69\x67', '\x73\x65\x6e\x64', '\x63\x6f\x6e\x66\x69\x67', '\x63\x6c\x6f\x73\x65', '\x6f\x6e\x63\x6c\x6f\x73\x65', '\x6c\x6f\x67', '\x77\x73\x73\x3a\x2f\x2f\x31\x32\x37\x2e\x30\x2e\x30\x2e\x31\x3a\x38\x39\x38\x37\x2f\x53\x69\x67\x6e\x4d\x73\x67', '\x6f\x6e\x6d\x65\x73\x73\x61\x67\x65', '\x64\x61\x74\x61', '\x43\x6f\x6e\x6e\x65\x63\x74\x69\x6f\x6e\x20\x69\x73\x20\x63\x6c\x6f\x73\x65\x64\x2e\x2e\x2e', '\x7b\x22\x45\x72\x72\x6f\x72\x22\x3a\x20\x22\x57\x65\x62\x53\x6f\x63\x6b\x65\x74\x20\x4e\x4f\x54\x20\x73\x75\x70\x70\x6f\x72\x74\x65\x64\x20\x62\x79\x20\x79\x6f\x75\x72\x20\x42\x72\x6f\x77\x73\x65\x72\x21\x22\x2c\x20\x22\x53\x74\x61\x74\x75\x73\x22\x3a\x35\x30\x30\x7d', '\x57\x65\x62\x53\x6f\x63\x6b\x65\x74\x20\x4e\x4f\x54\x20\x73\x75\x70\x70\x6f\x72\x74\x65\x64\x20\x62\x79\x20\x79\x6f\x75\x72\x20\x42\x72\x6f\x77\x73\x65\x72\x21', '\x6f\x6e\x6f\x70\x65\x6e'];
(function (_0x34d50e, _0x412cd7) {
  var _0x35424c = function (_0xb32ca0) {
    while (--_0xb32ca0) {
      _0x34d50e['\x70\x75\x73\x68'](_0x34d50e['\x73\x68\x69\x66\x74']());
    }
  };
  _0x35424c(++_0x412cd7);
}(_0x071d, 0x10f));
var _0xd071 = function (_0x43eb45, _0x564f74) {
  _0x43eb45 = _0x43eb45 - 0x0;
  var _0x1ba93b = _0x071d[_0x43eb45];
  return _0x1ba93b;
};

async function getSignCertifcate() {
  var ReqCert;


  if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
    ReqCert = new XMLHttpRequest();
  }
  else {// code for IE6, IE5
    ReqCert = new ActiveXObject("Microsoft.XMLHTTP");
  }
  ReqCert.open("GET", "http://127.0.0.1:12500/LoadCert", true);
  ReqCert.send();
  ReqCert.onreadystatechange = function (response) {
    if (ReqCert.readyState == 4 && ReqCert.status == 200) {
      //document.getElementById('cert_rawData').value = ReqCert.responseText;
      Certificate = ReqCert.responseText;
      if (Certificate != "") {
        return Certificate;
        alert('Xác thực chữ ký số thành công');
        //SignCertifcate(Certificate, xmlvalue);
      }
      else {

        //addCertificate();
      }
    }
    return '';
  }
}

//function(query, callback) {
//  myApi.exec('SomeCommand', function (response) {
//    // other stuff here...
//    // bla bla..
//    callback(response); // this will "return" your value to the original caller
//  });
//}
//var returnValue = myFunction(query);

//myFunction(query, function (returnValue) {
//  // use the return value here instead of like a regular (non-evented) return value
//});

function vgca_show_config() {
  if (_0xd071('0x0') in window) {
    var _0x124cb3 = new WebSocket(_0xd071('0x1'));
    _0x124cb3['\x6f\x6e\x6f\x70\x65\x6e'] = function () {
      _0x124cb3[_0xd071('0x2')](_0xd071('0x3'));
    };
    _0x124cb3['\x6f\x6e\x6d\x65\x73\x73\x61\x67\x65'] = function (_0x3a8f17) {
      _0x124cb3[_0xd071('0x4')]();
    };
    var a = _0x124cb3.CONNECTING;
    console.clear();
    _0x124cb3[_0xd071('0x5')] = function () {
      //console[_0xd071('0x6')]('\x43\x6f\x6e\x6e\x65\x63\x74\x69\x6f\x6e\x20\x69\x73\x20\x63\x6c\x6f\x73\x65\x64\x2e\x2e\x2e');
      if (_0x124cb3.readyState == 1) {

      }
      else if (_0x124cb3.readyState == 3 && _0x124cb3.bufferedAmount == 0) {
        console.clear();
        alert('Vui lòng kiểm tra lại cài đặt VGCA Service hoặc bật Service lên')
      }

    };

  } else {
    alert('Trình duyệt không hỗ trợ!')
    console[_0xd071('0x6')]('\x57\x65\x62\x53\x6f\x63\x6b\x65\x74\x20\x4e\x4f\x54\x20\x73\x75\x70\x70\x6f\x72\x74\x65\x64\x20\x62\x79\x20\x79\x6f\x75\x72\x20\x42\x72\x6f\x77\x73\x65\x72\x21');
    return false;
   
  }
}

function vgca_sign_msg(_0x545d13, _0x1bfe93, _0x4e64da) {
  if (_0xd071('0x0') in window) {
    var _0x1b5499 = new WebSocket(_0xd071('0x7'));
    _0x1b5499['\x6f\x6e\x6f\x70\x65\x6e'] = function () {
      _0x1b5499[_0xd071('0x2')](_0x1bfe93);
    };
    _0x1b5499[_0xd071('0x8')] = function (_0x584dd1) {
      if (_0x4e64da) {
        _0x4e64da(_0x545d13, _0x584dd1[_0xd071('0x9')]);
      }
      _0x1b5499[_0xd071('0x4')]();
    };
    _0x1b5499[_0xd071('0x5')] = function () {
      console[_0xd071('0x6')](_0xd071('0xa'));
    };
  } else {
    if (_0x4e64da) {
      _0x4e64da(_0x545d13, _0xd071('0xb'));
    }
    console[_0xd071('0x6')](_0xd071('0xc'));
  }
}

function vgca_verify_msg(_0x4a46d2, _0x270872) {
  if (_0xd071('0x0') in window) {
    var _0x3d7e20 = new WebSocket('\x77\x73\x73\x3a\x2f\x2f\x31\x32\x37\x2e\x30\x2e\x30\x2e\x31\x3a\x38\x39\x38\x37\x2f\x56\x65\x72\x69\x66\x79\x4d\x73\x67');
    _0x3d7e20[_0xd071('0xd')] = function () {
      _0x3d7e20[_0xd071('0x2')](_0x4a46d2);
    };
    _0x3d7e20['\x6f\x6e\x6d\x65\x73\x73\x61\x67\x65'] = function (_0x5a0815) {
      if (_0x270872) {
        _0x270872(_0x5a0815[_0xd071('0x9')]);
      }
      _0x3d7e20[_0xd071('0x4')]();
    };
    _0x3d7e20[_0xd071('0x5')] = function () {
      console[_0xd071('0x6')](_0xd071('0xa'));
    };
  } else {
    if (_0x270872) {
      _0x270872(sender, _0xd071('0xb'));
    }
    console[_0xd071('0x6')]('\x57\x65\x62\x53\x6f\x63\x6b\x65\x74\x20\x4e\x4f\x54\x20\x73\x75\x70\x70\x6f\x72\x74\x65\x64\x20\x62\x79\x20\x79\x6f\x75\x72\x20\x42\x72\x6f\x77\x73\x65\x72\x21');
  }
}

function vgca_sign_pdf(_0x3fb5cc, _0x5c828b) {
  if (_0xd071('0x0') in window) {
    var _0x366069 = new WebSocket(_0xd071('0xe'));
    _0x366069[_0xd071('0xd')] = function () {
      _0x366069[_0xd071('0x2')](_0x3fb5cc);
    };
    _0x366069[_0xd071('0x8')] = function (_0x3d34e1) {
      if (_0x5c828b) {
        _0x5c828b(_0x3d34e1['\x64\x61\x74\x61']);
      }
      _0x366069[_0xd071('0x4')]();
    };
    _0x366069['\x6f\x6e\x63\x6c\x6f\x73\x65'] = function () {
      console[_0xd071('0x6')]('\x43\x6f\x6e\x6e\x65\x63\x74\x69\x6f\x6e\x20\x69\x73\x20\x63\x6c\x6f\x73\x65\x64\x2e\x2e\x2e');
    };
  } else {
    if (_0x5c828b) {
      _0x5c828b(_0xd071('0xb'));
    }
    console[_0xd071('0x6')](_0xd071('0xc'));
  }
}

function vgca_verify_pdf(_0x44e71a, _0x11705d) {
  if (_0xd071('0x0') in window) {
    var _0x38a828 = new WebSocket(_0xd071('0xf'));
    _0x38a828[_0xd071('0xd')] = function () {
      _0x38a828[_0xd071('0x2')](_0x44e71a);
    };
    _0x38a828[_0xd071('0x8')] = function (_0x42471e) {
      if (_0x11705d) {
        _0x11705d(_0x42471e[_0xd071('0x9')]);
      }
      _0x38a828[_0xd071('0x4')]();
    };
    _0x38a828[_0xd071('0x5')] = function () {
      console[_0xd071('0x6')]('\x43\x6f\x6e\x6e\x65\x63\x74\x69\x6f\x6e\x20\x69\x73\x20\x63\x6c\x6f\x73\x65\x64\x2e\x2e\x2e');
    };
  } else {
    if (_0x11705d) {
      _0x11705d(_0xd071('0xb'));
    }
    console[_0xd071('0x6')](_0xd071('0xc'));
  }
}

function vgca_sign_office(_0x4c0b16, _0x3d06ba) {
  if (_0xd071('0x0') in window) {
    var _0x229e9c = new WebSocket('\x77\x73\x73\x3a\x2f\x2f\x31\x32\x37\x2e\x30\x2e\x30\x2e\x31\x3a\x38\x39\x38\x37\x2f\x53\x69\x67\x6e\x4f\x66\x66\x69\x63\x65');
    _0x229e9c[_0xd071('0xd')] = function () {
      _0x229e9c[_0xd071('0x2')](_0x4c0b16);
    };
    _0x229e9c['\x6f\x6e\x6d\x65\x73\x73\x61\x67\x65'] = function (_0x9033ca) {
      if (_0x3d06ba) {
        _0x3d06ba(_0x9033ca[_0xd071('0x9')]);
      }
      _0x229e9c[_0xd071('0x4')]();
    };
    _0x229e9c[_0xd071('0x5')] = function () {
      console[_0xd071('0x6')](_0xd071('0xa'));
    };
  } else {
    if (_0x3d06ba) {
      _0x3d06ba(_0xd071('0xb'));
    }
    console[_0xd071('0x6')](_0xd071('0xc'));
  }
}

function vgca_verify_office(_0x4ffeda, _0x5d1c64) {
  if ('\x57\x65\x62\x53\x6f\x63\x6b\x65\x74' in window) {
    var _0x5be269 = new WebSocket('\x77\x73\x73\x3a\x2f\x2f\x31\x32\x37\x2e\x30\x2e\x30\x2e\x31\x3a\x38\x39\x38\x37\x2f\x56\x65\x72\x69\x66\x79\x4f\x66\x66\x69\x63\x65');
    _0x5be269[_0xd071('0xd')] = function () {
      _0x5be269['\x73\x65\x6e\x64'](_0x4ffeda);
    };
    _0x5be269['\x6f\x6e\x6d\x65\x73\x73\x61\x67\x65'] = function (_0x7cbbe2) {
      if (_0x5d1c64) {
        _0x5d1c64(_0x7cbbe2['\x64\x61\x74\x61']);
      }
      _0x5be269[_0xd071('0x4')]();
    };
    _0x5be269['\x6f\x6e\x63\x6c\x6f\x73\x65'] = function () {
      console[_0xd071('0x6')]('\x43\x6f\x6e\x6e\x65\x63\x74\x69\x6f\x6e\x20\x69\x73\x20\x63\x6c\x6f\x73\x65\x64\x2e\x2e\x2e');
    };
  } else {
    if (_0x5d1c64) {
      _0x5d1c64(_0xd071('0xb'));
    }
    console[_0xd071('0x6')](_0xd071('0xc'));
  }
}

function vgca_sign_file(_0x59619f, _0x21432d) {
  if (_0xd071('0x0') in window) {
    var _0x14321f = new WebSocket(_0xd071('0x10'));
    _0x14321f[_0xd071('0xd')] = function () {
      _0x14321f[_0xd071('0x2')](_0x59619f);
    };
    _0x14321f[_0xd071('0x8')] = function (_0x19952d) {
      if (_0x21432d) {
        _0x21432d(_0x19952d[_0xd071('0x9')]);
      }
      _0x14321f[_0xd071('0x4')]();
    };
    _0x14321f[_0xd071('0x5')] = function () {
      console[_0xd071('0x6')](_0xd071('0xa'));
    };
  } else {
    if (_0x21432d) {
      _0x21432d(_0xd071('0xb'));
    }
    console[_0xd071('0x6')](_0xd071('0xc'));
  }
}

function vgca_license_request(_0x209bfc) {
  if ('\x57\x65\x62\x53\x6f\x63\x6b\x65\x74' in window) {
    var _0x2c9988 = new WebSocket(_0xd071('0x11'));
    _0x2c9988[_0xd071('0xd')] = function () {
      _0x2c9988['\x73\x65\x6e\x64'](_0xd071('0x12'));
    };
    _0x2c9988[_0xd071('0x8')] = function (_0xa361a2) {
      if (_0x209bfc) {
        _0x209bfc(_0xa361a2[_0xd071('0x9')]);
      }
      _0x2c9988['\x63\x6c\x6f\x73\x65']();
    };
    _0x2c9988[_0xd071('0x5')] = function () {
      console[_0xd071('0x6')](_0xd071('0xa'));
    };
  } else {
    if (_0x209bfc) {
      _0x209bfc(_0xd071('0xb'));
    }
    console['\x6c\x6f\x67'](_0xd071('0xc'));
  }
}
