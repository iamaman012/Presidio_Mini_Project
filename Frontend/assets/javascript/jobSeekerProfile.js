
const token = localStorage.getItem('token');
const userId = localStorage.getItem('UserId');
const jobSeekerId=localStorage.getItem('JobSeekerId')


if(localStorage.getItem('token')){
  document.getElementById('logOut').classList.remove('d-none');
  document.getElementById('login-resgister').classList.add('d-none');
}
if(localStorage.getItem('EmployerId')){
  document.getElementById('empProfile').classList.remove('d-none');
  
}
if(localStorage.getItem('JobSeekerId')){
  document.getElementById('jobProfile').classList.remove('d-none');
  
}
document.getElementById('logOut').addEventListener('click',()=>{
  localStorage.clear();
  window.location.href='login.html';
});
function formatDate(datetimeStr) {
  const date = new Date(datetimeStr);

  const options = { day: 'numeric', month: 'short', year: 'numeric' };
  return date.toLocaleDateString('en-GB', options);
}
const profileLoading = async () => {
  try {
    var response = await fetch(`http://localhost:5190/api/User/GetUserById?userid=${userId}`, {
      method: 'GET',
      headers: {
        'Authorization': 'Bearer ' + token,
      }
    });
    // Check if the response is okay (status in the range 200-299)
    if (!response.ok) {
      const errorData = await response.json(); // Convert error response to JSON

      throw new Error(`Status : ${errorData.statusCode}, message : ${errorData.message}`);
    }
    const data = await response.json();
    console.log(data);
    document.getElementById('name').innerText = data.name;
    document.getElementById('userName').innerText = data.name;
    document.getElementById('welcome').innerText = 'Welcome ' + data.name;
    document.getElementById('email').innerText = data.email;
    document.getElementById('role').innerText = data.role;
    document.getElementById('contact').innerText = data.contactNumber;
    document.getElementById('userImg').src = data.imageUrl;

  }
  catch (error) {
    console.log(error.message);
  }
}
const resumeLoading = async () => {
  try {

    var response = await fetch(`http://localhost:5190/api/JobSeeker/GetResumeById?jobSeekerId=${jobSeekerId}`, {
      method: 'GET',
      headers: {
        'Authorization': 'Bearer ' + token,
      }
    })
    if (!response.ok) {
      const errorData = await response.json(); // Convert error response to JSON

      throw new Error(`Status : ${errorData.statusCode}, message : ${errorData.message}`);
    }
    const data = await response.json();
    console.log(data);
    var Skills = document.getElementById('skills');
    Skills.innerHTML="";
    data.skills.forEach(skill => {
      const span = document.createElement('span');
      span.classList.add('badge');
      span.classList.add('bg-primary','text-light','me-1','mb-1');
      span.innerText = skill.skillName;
      Skills.appendChild(span);
    });
    var aboutEdu = document.getElementById('aboutEdu');
    aboutEdu.innerHTML="";
    data.educations.forEach(edu => {
      const div = document.createElement('span');
      div.classList.add('badge');
      div.classList.add('bg-primary','text-light','me-1','mb-1');
      div.innerText = edu.degree;
      aboutEdu.appendChild(div);
    });
    var aboutEdu= document.getElementById('aboutExp');
    aboutEdu.innerHTML="";
    data.experiences.forEach(exp => {
      const div = document.createElement('span');
      div.classList.add('badge');
      div.classList.add('bg-primary','text-light','me-1','mb-1');
      div.innerText = exp.jobTitle;
      aboutEdu.appendChild(div);
    });

    const education = document.getElementById('education-content');
    education.innerHTML="";
    data.educations.forEach(edu => {
      const div = document.createElement('div');
      div.classList.add('col-12', 'card');
      div.innerHTML = `
            <div class="row g-3">
              <div class="col-5 col-md-3 bg-light border-bottom border-white border-3">
                <div class="p-2">Institute</div>
              </div>
              <div class="col-7 col-md-9 bg-light border-start border-bottom border-white border-3">
                <div class="p-2" id="userName">${edu.institution}</div>
              </div>
              <div class="col-5 col-md-3 bg-light border-bottom border-white border-3">
                <div class="p-2">Degree</div>
              </div>
              <div class="col-7 col-md-9 bg-light border-start border-bottom border-white border-3">
                <div class="p-2" id="role">${edu.degree}</div>
              </div>
              <div class="col-5 col-md-3 bg-light border-bottom border-white border-3">
                <div class="p-2">Start Date</div>
              </div>

              <div class="col-7 col-md-9 bg-light border-start border-bottom border-white border-3">
                <div class="p-2" id="role">${formatDate(edu.startDate)}</div>
              </div>

              <div class="col-5 col-md-3 bg-light border-bottom border-white border-3">
                <div class="p-2">End Date</div>
              </div>

              <div class="col-7 col-md-9 bg-light border-start border-bottom border-white border-3">
                <div class="p-2" id="role">${formatDate(edu.endDate)}</div>
              </div>

              <div class="col-5 col-md-3 bg-light border-bottom border-white border-3">
                <div class="p-2">Description</div>
              </div>

              <div class="col-7 col-md-9 bg-light border-start border-bottom border-white border-3">
                <div class="p-2" id="role">${edu.description}</div>
              </div>

              <div class="col-5 col-md-3 bg-light border-bottom border-white border-3">
                <div class="p-2">GPA</div>
              </div>

              <div class="col-7 col-md-9 bg-light border-start border-bottom border-white border-3">
                <div class="p-2" id="role">${edu.gpa}</div>

              </div>

              <div class="col-5 col-md-3 bg-light border-bottom border-white border-3">
                <div class="p-2">Location</div>
              </div>
              <div class="col-7 col-md-9 bg-light border-start border-bottom border-white border-3">
                <div class="p-2" id="role">${edu.location}</div>
              </div>


              
              <div class="col-12">
                <button type="button" class="btn btn-primary btn-sm float-end me-1" onclick="updateEduModal(${edu.educationID},'${edu.institution}','${edu.degree}','${edu.startDate}','${edu.endDate}','${edu.description}',${edu.gpa},'${edu.location}')">Update</button>
                <button type="button" class="btn btn-dark btn-sm float-end me-2" onclick="delEdu(${edu.educationID})">Delete</button>
              </div>
            </div>
            `;
      education.appendChild(div);

    });
    const experience = document.getElementById('experience-content');
    experience.innerHTML="";
    data.experiences.forEach(exp => {
      const div = document.createElement('div');
      div.classList.add('col-12', 'card');
      div.innerHTML = `
            <div class="row g-3">
              <div class="col-5 col-md-3 bg-light border-bottom border-white border-3">
                <div class="p-2">Company </div>
              </div>
              <div class="col-7 col-md-9 bg-light border-start border-bottom border-white border-3">
                <div class="p-2" id="userName">${exp.companyName}</div>
              </div>
              <div class="col-5 col-md-3 bg-light border-bottom border-white border-3">
                <div class="p-2">Job Title</div>
              </div>
              <div class="col-7 col-md-9 bg-light border-start border-bottom border-white border-3">
                <div class="p-2" id="role">${exp.jobTitle}</div>
              </div>
              <div class="col-5 col-md-3 bg-light border-bottom border-white border-3">
                <div class="p-2">Start Date</div>
              </div>

              <div class="col-7 col-md-9 bg-light border-start border-bottom border-white border-3">
                <div class="p-2" id="role">${formatDate(exp.startDate)}</div>
              </div>

              <div class="col-5 col-md-3 bg-light border-bottom border-white border-3">
                <div class="p-2">End Date</div>
              </div>

              <div class="col-7 col-md-9 bg-light border-start border-bottom border-white border-3">
                <div class="p-2" id="role">${formatDate(exp.endDate)}</div>
              </div>

              <div class="col-5 col-md-3 bg-light border-bottom border-white border-3">
                <div class="p-2">Description</div>
              </div>

              <div class="col-7 col-md-9 bg-light border-start border-bottom border-white border-3">
                <div class="p-2" id="role">${exp.description}</div>
              </div>

              <div class="col-5 col-md-3 bg-light border-bottom border-white border-3">
                <div class="p-2">Location</div>
              </div>
              <div class="col-7 col-md-9 bg-light border-start border-bottom border-white border-3">
                <div class="p-2" id="role">${exp.location}</div>
              </div>



              <div class="col-12">
                <button type="button" class="btn btn-primary btn-sm float-end me-1" onclick="updateExp(${exp.experienceID}, '${exp.companyName}', '${exp.jobTitle}', '${exp.startDate}', '${exp.endDate}', '${exp.description}','${exp.location}')">Update</button>
                <button type="button" class="btn btn-dark btn-sm float-end me-2" onclick="delExp(${exp.experienceID})">Delete</button>
              </div>
            </div>
            `;
      experience.appendChild(div);
    });

    const skills = document.getElementById('skills-content');
    skills.innerHTML="";
    data.skills.forEach(skill => {
      const div = document.createElement('div');
      div.classList.add('col-12', 'card');
      div.innerHTML = `
            <div class="row g-3">
              <div class="col-5 col-md-3 bg-light border-bottom border-white border-3">
                <div class="p-2">Skill</div>
              </div>
              <div class="col-7 col-md-9 bg-light border-start border-bottom border-white border-3">
                <div class="p-2" id="userName">${skill.skillName}</div>
              </div>
              <div class="col-12">
                <button type="button" class="btn btn-primary btn-sm float-end me-1" onclick="updateSkill(${skill.skillID},'${skill.skillName}')">Update</button>
                <button type="button" class="btn btn-dark btn-sm float-end me-2" onclick="delSkill(${skill.skillID})">Delete</button>
              </div>
            </div>
            `;
      skills.appendChild(div);

    });


  }
  catch (error) {
    console.log(error.message);
  }

}


profileLoading();
resumeLoading();
const updatePassword = async (e) => {
  e.preventDefault()
  var currentPassword = document.getElementById('currentPassword').value;
  var newPassword = document.getElementById('newPassword').value;
 var confirmPassword = document.getElementById('confirmPassword').value;

  if (newPassword !== confirmPassword) {
    document.getElementById('errorNewPass').innerHTML = 'Password does not match';
    document.getElementById('errorNewPass').style.color = 'red';
    document.getElementById('errorConfirmPass').style.color = 'red';
    document.getElementById('errorConfirmPass').innerHTML = 'Password does not match';
    document.getElementById('newPassword').style.border = '1px solid red';
    document.getElementById('confirmPassword').style.border = '1px solid red';
    return;
  }



  await fetch(`http://localhost:5190/api/User/ChangePassword?userid=${userId}&oldPassword=${currentPassword}&newPassword=${newPassword}&confirmPassword=${confirmPassword}`, {
    method: 'PUT',
    headers: {
      'Authorization': 'Bearer ' + token,

    },

  }).then(response => {

    if (!response.ok) {

      return response.json().then(error => {
        throw new Error(error.message);
      });
    }
    return response;
  }).then(data => {
    console.log("aman")
    console.log(data);
    document.getElementById('toastMessage').innerText = "Password Changed Successfully";
    var toastElement = document.getElementById('myToast');
    var toast = new bootstrap.Toast(toastElement);
    toast.show();
    // Clear form values
    document.getElementById('currentPassword').value = '';
    document.getElementById('newPassword').value = '';
    document.getElementById('confirmPassword').value = '';
    
  }).catch(error => {
    console.log(error.message);
    document.getElementById('toastMessage').innerText = error.message;
    var toastElement = document.getElementById('myToast');
    var toast = new bootstrap.Toast(toastElement);
    toast.show();
  });
}
document.getElementById('passwordForm').addEventListener('submit',updatePassword)
const delEdu = async (eduId) => {
  try {
    var response = await fetch(`http://localhost:5190/api/JobSeeker/DeleteEducationById?educationId=${eduId}`, {
      method: 'DELETE',
      headers: {
        'Authorization': 'Bearer ' + token,
      }
    });
    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(`Status : ${errorData.statusCode}, message : ${errorData.message}`);
    }


    const data = await response.json();
    console.log(data);
    document.getElementById('toastMessage').innerText = "Education Deleted Successfully";
    var toastElement = document.getElementById('myToast');
    var toast = new bootstrap.Toast(toastElement);
    toast.show();
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    await resumeLoading();
    await profileLoading()
    // Show toast message

  }
  catch (error) {
    console.log(error.message);
  }
}
// for deleting experience
const delExp = async (expId) => {
  try {
    var response = await fetch(`http://localhost:5190/api/JobSeeker/DeleteExperienceById?experienceId=${expId}`, {
      method: 'DELETE',
      headers: {
        'Authorization': 'Bearer ' + token,
      }
    });
    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(`Status : ${errorData.statusCode}, message : ${errorData.message}`);
    }


    const data = await response.json();
    console.log(data);
    document.getElementById('toastMessage').innerText = "Experinece Deleted Successfully";
    var toastElement = document.getElementById('myToast');
    var toast = new bootstrap.Toast(toastElement);
    toast.show();
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    await resumeLoading();
    await profileLoading()

  }
  catch (error) {
    console.log(error.message);
  }
}
const addEdu = () => {
  const modal = new bootstrap.Modal(document.getElementById('educationModal'));
  modal.show();

  document.getElementById('updtEdu').style.display = 'none';
  document.getElementById('addEdu').style.display = 'block';
  const institution = document.getElementById('inputInstitution').value = "";
  const degree = document.getElementById('inputDegree').value = "";
  const startDate = document.getElementById('inputStartDate').value = "";
  const endDate = document.getElementById('inputEndDate').value = "";
  const description = document.getElementById('inputDescription').value = "";
  const gpa = document.getElementById('inputGPA').value = "";
  const location = document.getElementById('inputLocation').value = "";


}
const addEducation = async () => {
  const institution = document.getElementById('inputInstitution').value;
  const degree = document.getElementById('inputDegree').value;
  const startDate = document.getElementById('inputStartDate').value;
  const endDate = document.getElementById('inputEndDate').value;
  const description = document.getElementById('inputDescription').value;
  const gpa = document.getElementById('inputGPA').value;
  const location = document.getElementById('inputLocation').value;
  try {
    var response = await fetch("http://localhost:5190/api/JobSeeker/AddEducation", {
      method: 'POST',
      headers: {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        "jobSeekerId": localStorage.getItem('JobSeekerId'),
        "institution": institution,
        "degree": degree,
        "startDate": startDate,
        "endDate": endDate,
        "description": description,
        "gpa": gpa,
        "location": location
      })
    });
    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(`Status : ${errorData.statusCode}, message : ${errorData.message}`);
    }
    const data = await response.json();
    console.log(data);
    document.getElementById('toastMessage').innerText = "Education Added Successfully";
    var toastElement = document.getElementById('myToast');
    var toast = new bootstrap.Toast(toastElement);
    toast.show();
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    await resumeLoading();
    await profileLoading()
  }
  catch (error) {
    console.log(error.message);
  }
}

const expModal = () => {
  const modal = new bootstrap.Modal(document.getElementById('experienceModal'));
  modal.show();
  document.getElementById('updtExp').style.display = 'none';
  document.getElementById('addExp').style.display = 'block';
  const companyName = document.getElementById('Company').value = "";
  const jobTitle = document.getElementById('JobTitle').value = "";
  const startDate = document.getElementById('StartDate').value = "";
  const endDate = document.getElementById('EndDate').value = "";
  const description = document.getElementById('Description').value = "";
  const location = document.getElementById('Location').value = "";
}
const addExperience = async () => {
  const companyName = document.getElementById('Company').value;
  const jobTitle = document.getElementById('JobTitle').value;
  const startDate = document.getElementById('StartDate').value;
  const endDate = document.getElementById('EndDate').value;
  const description = document.getElementById('Description').value;
  const location = document.getElementById('Location').value;
  try {
    var response = await fetch("http://localhost:5190/api/JobSeeker/AddExperience", {
      method: 'POST',
      headers: {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        "jobSeekerId": localStorage.getItem('JobSeekerId'),
        "companyName": companyName,
        "jobTitle": jobTitle,
        "startDate": startDate,
        "endDate": endDate,
        "description": description,
        "location": location
      })
    });
    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(`Status : ${errorData.statusCode}, message : ${errorData.message}`);
    }
    const data = await response.json();
    console.log(data);
    document.getElementById('toastMessage').innerText = "Experience Added Successfully";
    var toastElement = document.getElementById('myToast');
    var toast = new bootstrap.Toast(toastElement);
    toast.show();
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    await resumeLoading();
    await profileLoading()
  }
  catch (error) {
    console.log(error.message);
  }

}
const delSkill = async (skillId) => {
  try {
    var response = await fetch(`http://localhost:5190/api/JobSeeker/DeleteSkillById?skillId=${skillId}`, {
      method: 'DELETE',
      headers: {
        'Authorization': 'Bearer ' + token,
      }
    });
    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(`Status : ${errorData.statusCode}, message : ${errorData.message}`);
    }
    const data = await response.json();
    console.log(data);
    document.getElementById('toastMessage').innerText = "Skill Deleted Successfully";
    var toastElement = document.getElementById('myToast');
    var toast = new bootstrap.Toast(toastElement);
    toast.show();
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    await resumeLoading();
    await profileLoading()
  }
  catch (error) {
    console.log(error.message);
  }
}
const skillModal = () => {
  const modal = new bootstrap.Modal(document.getElementById('skillsModal'));
  modal.show();
  document.getElementById('updateSkill').style.display = 'none';
  document.getElementById('addSkill').style.display = 'block';
}
const addSkill = async () => {
  const skillInput = document.getElementById('Skill').value;
  const skillsArray = skillInput.split(',').map(skill => skill.trim());
  try {
    var response = await fetch("http://localhost:5190/api/JobSeeker/AddSkills", {
      method: 'POST',
      headers: {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        "jobSeekerId": localStorage.getItem('JobSeekerId'),
        "skillNames": skillsArray
      })
    });
    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(`Status : ${errorData.statusCode}, message : ${errorData.message}`);
    }
    const data = await response.json();
    console.log(data);
    document.getElementById('toastMessage').innerText = "Skill Added Successfully";
    var toastElement = document.getElementById('myToast');
    var toast = new bootstrap.Toast(toastElement);
    toast.show();
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    await resumeLoading();
    await profileLoading()
  }
  catch (error) {
    console.log(error.message);
  }
}
var updateSkillID = 0;
const updateSkill = async(skillId, skillName) => {
  try{
    const modal = new bootstrap.Modal(document.getElementById('skillsModal'));
    modal.show();
    document.getElementById('Skill').value = skillName;
    document.getElementById('addSkill').style.display = 'none';
    document.getElementById('updateSkill').style.display = 'block';
    updateSkillID = skillId;
  }
  catch(error){
    console.log(error.message);
  }
}
const updateDbSkill = async() => {
  try{
    var skillName = document.getElementById('Skill').value;
    var response = await fetch(`http://localhost:5190/api/JobSeeker/UpdateSkill?jobSeekerId=${jobSeekerId}&skillId=${updateSkillID}&skillName=${skillName}`, {
      method: 'PUT',
      headers: {
        'Authorization': 'Bearer ' + token,
      }
    });
    if(!response.ok){
      const errorData = await response.json();
      throw new Error(`Status : ${errorData.statusCode}, message : ${errorData.message}`);
    }
    const data = await response.json();
    console.log(data);
    document.getElementById('toastMessage').innerText = "Skill Updated Successfully";
    var toastElement = document.getElementById('myToast');
    var toast = new bootstrap.Toast(toastElement);
    toast.show();
    await new Promise(resolve => setTimeout(resolve, 1000));
    await resumeLoading();
    await profileLoading();
    
  }
  catch(error){
    console.log(error.message);
  }
}

document.getElementById('deleteIcon').addEventListener('click', async () => {
  const confirmDelete = confirm("Are you sure you want to delete the profile?");
  if (confirmDelete) {
      try {
          const response = await fetch(`http://localhost:5190/api/User/DeleteUserById?userid=${userId}`, {
              method: 'DELETE',
              headers: {
                  'Authorization': 'Bearer ' + token, // Include token for authentication
                  'Content-Type': 'application/json'
              },
          });
          if (!response.ok) {
              const errorData = await response.json();
              throw new Error(`Status: ${errorData.statusCode}, message: ${errorData.message}`);
          }
          document.getElementById('toastMessage').innerText = 'Profile Deleted Successfully';
              var toastElement = document.getElementById('myToast');
              var toast = new bootstrap.Toast(toastElement);
              toast.show();
             
              await new Promise(resolve => setTimeout(resolve, 1000));
              localStorage.clear();
              window.location.href = 'login.html';

      } catch (error) {
          console.error('Error deleting profile:', error);
          alert('Error deleting profile. Please try again.');
      }
  }
});


var experienceId = 0;
const updateExp = (expId, companyName, jobTitle, startDate, endDate, description, location) => {
  try {
    // Ensure the date values are in 'yyyy-MM-dd' format
    const formatDate = (date) => {
      const d = new Date(date);
      const year = d.getFullYear();
      const month = String(d.getMonth() + 1).padStart(2, '0');
      const day = String(d.getDate()).padStart(2, '0');
      return `${year}-${month}-${day}`;
    };

    const formattedStartDate = formatDate(startDate);
    const formattedEndDate = formatDate(endDate);

    console.log(expId, companyName, jobTitle, formattedStartDate, formattedEndDate, description, location);
    const modal = new bootstrap.Modal(document.getElementById('experienceModal'));
    modal.show();
    document.getElementById('addExp').style.display = 'none';
    document.getElementById('updtExp').style.display = 'block';

    document.getElementById('Company').value = companyName;
    document.getElementById('JobTitle').value = jobTitle;
    document.getElementById('StartDate').value = formattedStartDate;
    document.getElementById('EndDate').value = formattedEndDate;
    document.getElementById('Description').value = description;
    document.getElementById('Location').value = location;
    experienceId = expId;
  } catch (error) {
    console.log(error.message);
  }
};
const updateExpereinnce = async () => {
  try {
    const companyName = document.getElementById('Company').value;
    const jobTitle = document.getElementById('JobTitle').value;
    const startDate = document.getElementById('StartDate').value;
    const endDate = document.getElementById('EndDate').value;
    const description = document.getElementById('Description').value;
    const location = document.getElementById('Location').value;
    var response = await fetch("http://localhost:5190/api/JobSeeker/UpdateExperience", {
      method: 'PUT',
      headers: {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        "experienceID": experienceId,
        "jobSeekerId": localStorage.getItem('JobSeekerId'),
        "companyName": companyName,
        "jobTitle": jobTitle,
        "startDate": startDate,
        "endDate": endDate,
        "description": description,
        "location": location
      })
    });
    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(`Status : ${errorData.statusCode}, message : ${errorData.message}`);
    }
    const data = await response.json();
    console.log(data);
    document.getElementById('toastMessage').innerText = "Update Experience Successfully";
    var toastElement = document.getElementById('myToast');
    var toast = new bootstrap.Toast(toastElement);
    toast.show();
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    await resumeLoading();
    await profileLoading()
  }
  catch (error) {
    console.log(error.message);

  }
}
var educationId = 0;

const updateEduModal = (eduId, institution, degree, startDate, endDate, description, gpa, location) => {
  try {
    // Ensure the date values are in 'yyyy-MM-dd' format
    const formatDate = (date) => {
      const d = new Date(date);
      const year = d.getFullYear();
      const month = String(d.getMonth() + 1).padStart(2, '0');
      const day = String(d.getDate()).padStart(2, '0');
      return `${year}-${month}-${day}`;
    };

    const formattedStartDate = formatDate(startDate);
    const formattedEndDate = formatDate(endDate);

    console.log(eduId, institution, degree, formattedStartDate, formattedEndDate, description, gpa, location);
    const modal = new bootstrap.Modal(document.getElementById('educationModal'));
    modal.show();
    document.getElementById('addEdu').style.display = 'none';
    document.getElementById('updtEdu').style.display = 'block';

    document.getElementById('inputInstitution').value = institution;
    document.getElementById('inputDegree').value = degree;
    document.getElementById('inputStartDate').value = formattedStartDate;
    document.getElementById('inputEndDate').value = formattedEndDate;
    document.getElementById('inputDescription').value = description;
    document.getElementById('inputGPA').value = gpa;
    document.getElementById('inputLocation').value = location;

    educationId = eduId;
  } catch (error) {
    console.log(error.message);
  }
}
const updateEducation = async () => {
  try {
    const institution = document.getElementById('inputInstitution').value;
    const degree = document.getElementById('inputDegree').value;
    const startDate = document.getElementById('inputStartDate').value;
    const endDate = document.getElementById('inputEndDate').value;
    const description = document.getElementById('inputDescription').value;
    const gpa = document.getElementById('inputGPA').value;
    const location = document.getElementById('inputLocation').value;
    var response = await fetch("http://localhost:5190/api/JobSeeker/UpdateEducation", {
      method: 'PUT',
      headers: {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        "educationID": educationId,
        "jobSeekerId": localStorage.getItem('JobSeekerId'),
        "institution": institution,
        "degree": degree,
        "startDate": startDate,
        "endDate": endDate,
        "description": description,
        "gpa": gpa,
        "location": location
      })
    });
    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(`Status : ${errorData.statusCode}, message : ${errorData.message}`);
    }
    const data = await response.json();
    console.log(data);
    document.getElementById('toastMessage').innerText = " Update Education  Successfully";
    var toastElement = document.getElementById('myToast');
    var toast = new bootstrap.Toast(toastElement);
    toast.show();
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    await resumeLoading();
    await profileLoading()
  }
  catch (error) {
    console.log(error.message);

  }
}

const applicationLoading = async () => {
  try {
    const token = localStorage.getItem('token');
    const jobSeekerId = localStorage.getItem('JobSeekerId');

    if (!token || !jobSeekerId) {
      throw new Error('Token or Job Seeker ID is missing');
    }

    const response = await fetch(`http://localhost:5190/api/Application/GetApplicationFilteredByJobSeekerID?jobSeekerID=${jobSeekerId}`, {
      method: 'GET',
      headers: {
        'Authorization': 'Bearer ' + token,
      }
    });

    if (!response.ok) {
      var application = document.getElementById('applications-tab-pane');
      application.innerHTML = '';
      const errorData = await response.json();
      throw new Error(`Status : ${errorData.statusCode}, message : ${errorData.message}`);
    }

    const data = await response.json();
    console.log(data);

    var application = document.getElementById('applications-tab-pane');
    application.innerHTML = '';

    data.forEach(job => {
      const jobItem = document.createElement('div');
      jobItem.classList.add('job-item', 'p-4', 'mb-4');
      jobItem.innerHTML = `
        <div class="row g-4">
          <div class="col-sm-12 col-md-8 d-flex align-items-center">
              <img class="flex-shrink-0 img-fluid border rounded" src="${job.companyImage}" alt="" style="width: 80px; height: 80px;">
              <div class="text-start ps-4">
                  <h5 class="mb-3">${job.jobTitle}</h5>
                  <span class="text-truncate me-3"><i class="fa fa-map-marker-alt text-primary me-2"></i>${job.location}</span>
                  <span class="text-truncate me-3"><i class="far fa-clock text-primary me-2"></i>${job.jobType}</span>
                  <span class="text-truncate me-3"><i class="far fa-money-bill-alt text-primary me-2"></i>${job.salary}</span>
                  <span class="text-truncate me-3"><i class="far fa-check-circle text-primary me-2"></i>${job.status}</span>
              </div>
          </div>
          <div class="col-sm-12 col-md-4 d-flex flex-column align-items-start align-items-md-end justify-content-center">
              <div class="d-flex mb-3">
                   <button class="btn btn-dark" onclick="delApplication(${job.applicationID},event)">Delete</button>
              </div>
              <small class="text-truncate"><i class="far fa-calendar-alt text-primary me-2"></i>Date Line: ${formatDate(job.applicationDate)}</small>
          </div>
        </div>
      `;
      jobItem.addEventListener('click', async() => {
        await displayJobDetails(job.jobID);
    });
      application.appendChild(jobItem);
    });
  } catch (error) {
    console.log(error.message);
    // Optionally display the error to the user in the UI
    var application = document.getElementById('applications-tab-pane');
    application.innerHTML = `<div class="alert alert-danger" role="alert">${error.message}</div>`;
  }
}
const displayJobDetails = async(jobID) => {
 try{
  
  const response = await fetch(`http://localhost:5190/api/JobListing/GetJobListingByJobID?jobID=${jobID}`, {
      method: 'GET',
      headers: {
          'Authorization': 'Bearer ' + token,
      }
  });
  if(!response.ok) {
      const errorData = await response.json();
      throw new Error(`Status : ${errorData.statusCode}, message : ${errorData.message}`);
  }
  const job = await response.json();
  console.log(job);

  document.getElementById('jobModalLabel').innerText = job.jobTitle;
  document.getElementById('jobModalImage').src = job.imageUrl;
  document.getElementById('jobModalCompanyName').innerText = job.companyName;
  document.getElementById('jobModalJobTitle').innerText = job.jobTitle;
  document.getElementById('jobModalCategory').innerText = job.category;
  document.getElementById('jobModalDescription').innerText = job.jobDescription;
  document.getElementById('jobModalSalary').innerText = job.salary;
  document.getElementById('jobModalLocation').innerText = job.location;
  document.getElementById('jobModalType').innerText = job.jobType;
  document.getElementById('jobModalPostingDate').innerText = formatDate(job.postingDate);
  document.getElementById('jobModalClosingDate').innerText = formatDate(job.closingDate);

  // Display skills as a list
  const skillsList = document.getElementById('jobModalSkills');
  skillsList.innerHTML = ''; // Clear previous skills
  job.skills.forEach(skill => {
      const skillItem = document.createElement('li');
      skillItem.innerText = skill.skillName;
      skillsList.appendChild(skillItem);
  });

  const jobModal = new bootstrap.Modal(document.getElementById('jobModal'));
  jobModal.show();
}
catch(error){
  console.log(error.message);
}
}



applicationLoading();
const delApplication = async (appId,event) => {
  try {
    event.stopPropagation();
    var response = await fetch(`http://localhost:5190/api/Application/DeleteApplicationById?applicationId=${appId}`, {
      method: 'DELETE',
      headers: {
        'Authorization': 'Bearer ' + token,
      }
    });
    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(`Status : ${errorData.statusCode}, message : ${errorData.message}`);
    }
    const data = await response.json();
    console.log(data);
    document.getElementById('toastMessage').innerText = "Application Deleted";
    var toastElement = document.getElementById('myToast');
    var toast = new bootstrap.Toast(toastElement);
    toast.show();
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    await applicationLoading()
  }
  catch (error) {
    console.log(error.message);
  }
}
const postJob=async()=>{
  try{
      const employerId = localStorage.getItem('EmployerId');
      if (!employerId) {
          
         
          document.getElementById('toastMessage').innerText = 'Please login as a Employer to post a job';
          var toastElement = document.getElementById('myToast');
          var toast = new bootstrap.Toast(toastElement);
          toast.show();
          await new Promise(resolve => setTimeout(resolve, 1000));
              
          
        
          
          return;
      }
      window.location.href='jobPosting.html';
  }
  catch(err){
      console.log(err.message);
  }
}
