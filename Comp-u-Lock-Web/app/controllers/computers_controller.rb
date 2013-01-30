class ComputersController < ApplicationController
  before_filter :authenticate_user!#, :except => [:index, :list]
  def index
  	list
  end

  def edit
  end

  # display all current user computers
  def list
  		user = User.find(current_user.id)
  	 	@computers = user.computer.all
  end

  def update

  end

  def save

  end

  def show

  end
end
