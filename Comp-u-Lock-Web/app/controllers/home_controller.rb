class HomeController < ApplicationController
  def index
  	unless current_user.nil?
	  	@user = User.find(current_user.id)
	  	unless @user.nil?
	  		emailar = @user.email.split('@')
	  		@email = emailar[0].humanize
	  	end
  	end
  end
end
