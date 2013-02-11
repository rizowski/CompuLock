class ComputerController < ApplicationController
    before_filter :authenticate_user!#, :except => [:index, :list]
    load_and_authorize_resource
    
    def index
  	 list
    end

    def edit
      @computer = Computer.find(params[:id])
    end

  # display all current user computers
    def list
  		user = User.find(current_user.id)
  	  @computers = user.computer.all
    end

    def update
      @computer = Computer.find(params[:id])
      if @computer.update_attributes params[:computer]
        flash[:notice] = "Information for your computer has been updated."
        redirect_to action: 'edit', id: @computer.id
      else
        flash[:notice] = "Something went wrong with saving computer information."
        redirect_to action: 'edit', id: @computer.id
      end
    end

    def destroy
      @computer = Computer.find(params[:id])
      if can? :destroy, @computer
        @computer.delete
      else
        flash[:notice] = "You can not delete this computer."
      end
      redirect_to(:action => 'index')
    end

    def show
      @computer = Computer.find(params[:id])
    end
end
